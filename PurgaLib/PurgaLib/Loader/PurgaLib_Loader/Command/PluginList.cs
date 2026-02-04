using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using PurgaLib.Loader.PurgaLib_Loader.LoaderEvent;
using System.Reflection;

namespace PurgaLib.Loader.PurgaLib_Loader.Command
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PluginList : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
        {
            if (PurgaLoader.LoadedPlugins.Count == 0)
            {
                response = "[PurgaLib] No plugins loaded";
                return true;
            }

            response = "[PurgaLib] Loaded plugins:\n";

            foreach (var plugin in PurgaLoader.LoadedPlugins)
            {
                var type = plugin.GetType();

                string name = GetProp(plugin, "Name");
                string version = GetProp(plugin, "Version");
                string author = GetProp(plugin, "Author");
                string description = GetProp(plugin, "Description");

                response += $" - {name} v{version} by {author}\n {description}";
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
        public string Description => "Shows all plugins loaded in PurgaLib";
    }
}