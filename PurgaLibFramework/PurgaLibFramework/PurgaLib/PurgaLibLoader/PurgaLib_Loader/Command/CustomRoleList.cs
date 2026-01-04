using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomRoles.Handlers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.Command
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ListRolesCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
        {
            if (!CustomRoleHandler.Registered.Any())
            {
                response = "No custom roles are registered.";
                return true;
            }

            response = "Registered custom roles: " + string.Join(", ", CustomRoleHandler.Registered.Select(r => $"{r.Id} ({r.Name})"));
            return true;
        }

        public string Command { get; } = "listroles";
        public string[] Aliases { get; } = Array.Empty<string>();
        public string Description { get; } = "Lists all registered custom roles.";
    }
}