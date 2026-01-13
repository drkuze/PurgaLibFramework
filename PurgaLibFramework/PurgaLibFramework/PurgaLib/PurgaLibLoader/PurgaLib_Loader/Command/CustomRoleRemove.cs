using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using LabApi.Features.Wrappers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomRoles;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomRoles.Handlers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.Command
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CustomRoleRemove : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
        {
            response = string.Empty;

            if (arguments.Count < 2)
            {
                response = "Usage: removerole <player> <roleId>";
                return false;
            }

            var args = arguments.ToArray();
            string targetName = args[0];
            string roleId = args[1];

            Player target = Player.List.FirstOrDefault(p => p.Nickname.Equals(targetName, StringComparison.OrdinalIgnoreCase));
            if (target == null)
            {
                response = $"Player '{targetName}' not found.";
                return false;
            }

            CustomRole role = CustomRoleHandler.Registered.FirstOrDefault(r => r.Id.Equals(roleId, StringComparison.OrdinalIgnoreCase));
            if (role == null)
            {
                response = $"Role '{roleId}' does not exist.";
                return false;
            }

            CustomRoleHandler.Remove(target, role);

            response = $"Player '{target.Nickname}' had role '{role.Name}' removed.";
            return true;
        }


        public string Command { get; } = "removerole";
        public string[] Aliases { get; } = Array.Empty<string>();
        public string Description { get; } = "Removes a custom role from a player.";
    }
}