using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PurgaLib.API.Features.PluginManager;
using PurgaLib.API.Features.Server;

namespace PurgaLib.Loader.PurgaLib_Loader.LoaderEvent
{
    public class PurgaLoader
    {
        public static readonly List<object> LoadedPlugins = new();

        private readonly string _purgaLibFolder;
        private readonly string _pluginFolder;
        private readonly string _configRootFolder;

        public PurgaLoader()
        {
            _purgaLibFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PurgaLib");
            _pluginFolder = Path.Combine(_purgaLibFolder, "Plugins");
            _configRootFolder = Path.Combine(_purgaLibFolder, "Config");

            Directory.CreateDirectory(_purgaLibFolder);
            Directory.CreateDirectory(_pluginFolder);
            Directory.CreateDirectory(_configRootFolder);

            Logged.SendRaw("[PurgaLib]: Loader Enabled.", ConsoleColor.Magenta);
        }

        public void LoadAllPlugins()
        {
            LoadExternalPlugins();
            LoadInternalPlugins();
        }

        public void LoadExternalPlugins()
        {
            int serverPort = LabApi.Features.Wrappers.Server.Port;

            string globalPluginFolder = _pluginFolder;
            string globalConfigFolder = _configRootFolder;
            string serverPluginFolder = Path.Combine(_pluginFolder, serverPort.ToString());
            string serverConfigFolder = Path.Combine(_configRootFolder, serverPort.ToString());

            Directory.CreateDirectory(serverPluginFolder);
            Directory.CreateDirectory(serverConfigFolder);

            LoadFromFolder(globalPluginFolder, globalConfigFolder);
            LoadFromFolder(serverPluginFolder, serverConfigFolder);
        }

        private void LoadFromFolder(string pluginFolder, string configFolder)
        {
            var files = Directory.GetFiles(pluginFolder, "*.dll");
            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    var pluginTypes = assembly.GetTypes()
                        .Where(t => t.IsClass && !t.IsAbstract && IsPluginType(t));

                    foreach (var type in pluginTypes)
                    {
                        RegisterPlugin(type, configFolder);
                    }
                }
                catch (Exception ex)
                {
                    Logged.SendRaw($"Error loading plugin DLL {file}: {ex.Message}", ConsoleColor.Red);
                }
            }
        }

        public void LoadInternalPlugins()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var pluginTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && IsPluginType(t));

            foreach (var type in pluginTypes)
            {
                RegisterPlugin(type, _configRootFolder);
            }
        }

        private void RegisterPlugin(Type type, string configRootFolder)
        {
            try
            {
                var pluginInstance = Activator.CreateInstance(type);
                string pluginName = GetProperty(pluginInstance, "Name");

                var configProperty = type.GetProperty("Config");
                if (configProperty != null && typeof(IConfig).IsAssignableFrom(configProperty.PropertyType))
                {
                    var pluginConfigFolder = Path.Combine(configRootFolder, pluginName);
                    Directory.CreateDirectory(pluginConfigFolder);

                    var configPath = Path.Combine(pluginConfigFolder, "config.yml");
                    var configValue = ConfigManager.LoadConfig(configProperty.PropertyType, configPath);
                    configProperty.SetValue(pluginInstance, configValue);
                }

                var requireProp = type.GetProperty("RequiredPurgaLibVersion");
                if (requireProp != null && typeof(Version).IsAssignableFrom(requireProp.PropertyType))
                {
                    var requiredVersion = (Version)requireProp.GetValue(pluginInstance);
                    if (requiredVersion > Loader.Instance.Version)
                        return;
                }

                LoadedPlugins.Add(pluginInstance);

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
                    var onEnabled = type.GetMethod("OnEnabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    Logged.SetPlugin(pluginName);
                    onEnabled?.Invoke(pluginInstance, null);
                    Logged.ClearPlugin();
                }

                string pluginAuthor = GetProperty(pluginInstance, "Author");
                string pluginVersion = GetProperty(pluginInstance, "Version");

                Logged.SendRaw($"[PurgaLib] [{pluginName}] Loaded v{pluginVersion} by {pluginAuthor}", ConsoleColor.Magenta);
            }
            catch (Exception ex)
            {
                Logged.SendRaw($"Error registering plugin {type.Name}: {ex.Message}", ConsoleColor.Red);
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
            var prop = plugin.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
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
                        var pluginConfigFolder = Path.Combine(_configRootFolder, pluginName);
                        Directory.CreateDirectory(pluginConfigFolder);

                        var configPath = Path.Combine(pluginConfigFolder, "config.yml");
                        ConfigManager.SaveConfig(configPath, configValue);
                    }

                    var onDisabled = type.GetMethod("OnDisabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
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
            Logged.SendRaw("[PurgaLib] All plugins disabled", ConsoleColor.White);
        }

        public string PluginFolder => _pluginFolder;
        public string ConfigFolder => _configRootFolder;

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