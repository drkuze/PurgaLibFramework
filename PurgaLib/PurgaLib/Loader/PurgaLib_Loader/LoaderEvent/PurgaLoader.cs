using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PurgaLib.API.Core;
using PurgaLib.API.Features.PluginManager;
using PurgaLib.API.Features.Server;

namespace PurgaLib.Loader.PurgaLib_Loader.LoaderEvent
{
    public class PurgaLoader
    {
        public static readonly List<object> LoadedPlugins = new();

        private readonly PEventRegister _register = new();

        public PurgaLoader()
        {
            PurgaPaths.Reload();

            Logged.SendRaw(
                $"[PurgaLib]: Loader Enabled on port {PurgaPaths.ServerPort}.",
                ConsoleColor.Magenta);
        }

        public void LoadAllPlugins()
        {
            LoadExternalPlugins();
            LoadInternalPlugins();
        }

        public void LoadExternalPlugins()
        {
            LoadFromFolder(PurgaPaths.GlobalPlugins);
            LoadFromFolder(PurgaPaths.ServerPlugins);
        }

        private void LoadFromFolder(string pluginFolder)
        {
            if (!Directory.Exists(pluginFolder))
                return;

            var files = Directory.GetFiles(pluginFolder, "*.dll");

            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    _register.RegisterAssembly(assembly);

                    var pluginTypes = assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && IsPluginType(t));

                    foreach (var type in pluginTypes)
                        RegisterPlugin(type);
                }
                catch (Exception ex)
                {
                    Logged.SendRaw(
                        $"Error loading plugin DLL {file}: {ex.Message}",
                        ConsoleColor.Red);
                }
            }
        }

        public void LoadInternalPlugins()
        {
            var assembly = Assembly.GetExecutingAssembly();
            _register.RegisterAssembly(assembly);

            var pluginTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && IsPluginType(t));

            foreach (var type in pluginTypes)
                RegisterPlugin(type);
        }

        private void RegisterPlugin(Type type)
        {
            try
            {
                var pluginInstance = Activator.CreateInstance(type);
                string pluginName = GetProperty(pluginInstance, "Name");
                
                var configProperty = type.GetProperty("Config");
                if (configProperty != null &&
                    typeof(IConfig).IsAssignableFrom(configProperty.PropertyType))
                {
                    var configPath = PurgaPaths.GetPluginConfigPath(pluginName);

                    var configValue = ConfigManager.LoadConfig(
                        configProperty.PropertyType,
                        configPath);

                    configProperty.SetValue(pluginInstance, configValue);
                }
                
                var requireProp = type.GetProperty("RequiredPurgaLibVersion");
                if (requireProp != null &&
                    typeof(Version).IsAssignableFrom(requireProp.PropertyType))
                {
                    var requiredVersion = (Version)requireProp.GetValue(pluginInstance);

                    if (requiredVersion > Loader.Instance.Version)
                        return;
                }

                LoadedPlugins.Add(pluginInstance);
                CommandLoader.RegisterCommands(pluginInstance);
                
                bool enabled = true;

                if (configProperty != null)
                {
                    var enabledProp = configProperty.PropertyType.GetProperty("Enabled");
                    var cfg = configProperty.GetValue(pluginInstance);

                    if (enabledProp != null && enabledProp.GetValue(cfg) is bool b)
                        enabled = b;
                }

                if (enabled)
                {
                    var onEnabled = type.GetMethod(
                        "OnEnabled",
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic);

                    Logged.SetPlugin(pluginName);
                    onEnabled?.Invoke(pluginInstance, null);
                    Logged.ClearPlugin();
                }

                string pluginAuthor = GetProperty(pluginInstance, "Author");
                string pluginVersion = GetProperty(pluginInstance, "Version");

                Logged.SendRaw(
                    $"[PurgaLib] [{pluginName}] Loaded v{pluginVersion} by {pluginAuthor}",
                    ConsoleColor.Magenta);
            }
            catch (Exception ex)
            {
                Logged.SendRaw(
                    $"Error registering plugin {type.Name}: {ex.Message}",
                    ConsoleColor.Red);
            }
        }

        private bool IsPluginType(Type type)
        {
            var baseType = type.BaseType;
            if (baseType == null) return false;
            if (!baseType.IsGenericType) return false;

            return baseType.GetGenericTypeDefinition() == typeof(Plugin<>);
        }

        private string GetProperty(object plugin, string propertyName)
        {
            var prop = plugin.GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            var value = prop?.GetValue(plugin);
            return value?.ToString() ?? "(unknown)";
        }

        public void UnloadPlugins()
        {
            foreach (var plugin in LoadedPlugins)
            {
                try
                {
                    var type = plugin.GetType();
                    string pluginName = GetProperty(plugin, "Name");

                    var configProperty = type.GetProperty("Config");
                    if (configProperty != null)
                    {
                        var configValue = configProperty.GetValue(plugin);
                        var configPath = PurgaPaths.GetPluginConfigPath(pluginName);

                        ConfigManager.SaveConfig(configPath, configValue);
                    }

                    CommandLoader.UnregisterCommands(plugin);

                    var onDisabled = type.GetMethod(
                        "OnDisabled",
                        BindingFlags.Instance |
                        BindingFlags.Public |
                        BindingFlags.NonPublic);

                    Logged.SetPlugin(pluginName);
                    onDisabled?.Invoke(plugin, null);
                    Logged.ClearPlugin();
                }
                catch
                {
                    // ignored
                }
            }

            LoadedPlugins.Clear();
            _register.UnRegisterAll();

            Logged.SendRaw("[PurgaLib] All plugins disabled", ConsoleColor.White);
        }

        public string PluginFolder => PurgaPaths.GlobalPlugins;
        public string ConfigFolder => PurgaPaths.ServerConfigs;

        public void ListPlugins()
        {
            Logged.SendRaw("[PurgaLib] Plugins:", ConsoleColor.Magenta);

            foreach (var plugin in LoadedPlugins)
            {
                string pluginName = GetProperty(plugin, "Name");
                Logged.SendRaw($" - {pluginName}", ConsoleColor.Magenta);
            }
        }
    }
}