using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.LoaderEvent;
using System.Reflection;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.Command
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PluginList : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
        {
            if (PurgaLoader.LoadedPlugins.Count == 0)
            {
                response = "[PurgaLibFramework] No plugins loaded";
                return true;
            }

            response = "[PurgaLibFramework] Loaded plugins:\n";

            foreach (var plugin in PurgaLoader.LoadedPlugins)
            {
                var type = plugin.GetType();

                string name = GetProp(plugin, "Name");
                string version = GetProp(plugin, "Version");
                string author = GetProp(plugin, "Author");

                response += $" - {name} v{version} by {author}\n";
            }

            return true;
        }

        private string GetProp(object obj, string prop)
        {
            var p = obj.GetType().GetProperty(prop, BindingFlags.Public | BindingFlags.Instance);
            var v = p?.GetValue(obj);
            return v?.ToString() ?? "Unknown";
        }

        public string Command => "purgalist";
        public string[] Aliases => new[] { "plist" };
        public string Description => "Shows all plugins loaded in PurgaLibFramework";
    }
}