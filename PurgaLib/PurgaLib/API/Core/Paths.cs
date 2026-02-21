using System;
using System.IO;
using LabApi.Features.Wrappers;

namespace PurgaLib.API.Core
{
    public static class PurgaPaths
    {
        static PurgaPaths() => Reload();

        public static int ServerPort => Server.Port;

        public static string AppData { get; } =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static string Root { get; private set; }

        public static string Plugins { get; private set; }

        public static string GlobalPlugins { get; private set; }

        public static string ServerPlugins { get; private set; }

        public static string Configs { get; private set; }

        public static string ServerConfigs { get; private set; }

        public static void Reload(string rootDirectory = null)
        {
            rootDirectory ??= Path.Combine(AppData, "PurgaLib");

            Root = rootDirectory;

            Plugins = Path.Combine(Root, "Plugins");
            GlobalPlugins = Plugins;
            ServerPlugins = Path.Combine(Plugins, ServerPort.ToString());

            Configs = Path.Combine(Root, "Configs");
            ServerConfigs = Path.Combine(Configs, ServerPort.ToString());
            
            Directory.CreateDirectory(Root);
            Directory.CreateDirectory(Plugins);
            Directory.CreateDirectory(ServerPlugins);
            Directory.CreateDirectory(Configs);
            Directory.CreateDirectory(ServerConfigs);
        }

        public static string GetPluginConfigFolder(string pluginName)
        {
            string folder = Path.Combine(ServerConfigs, pluginName);
            Directory.CreateDirectory(folder);
            return folder;
        }

        public static string GetPluginConfigPath(string pluginName)
        {
            return Path.Combine(GetPluginConfigFolder(pluginName), "config.yml");
        }

        public static string GetPluginTranslationPath(string pluginName)
        {
            return Path.Combine(GetPluginConfigFolder(pluginName), "translations.yml");
        }
    }
}