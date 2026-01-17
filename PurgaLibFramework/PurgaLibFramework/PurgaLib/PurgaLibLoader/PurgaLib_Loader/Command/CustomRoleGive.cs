using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommandSystem;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomRoles;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomRoles.Handlers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.Command;

[CommandHandler(typeof(GameConsoleCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class CustomRoleGive : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        response = string.Empty;

        if (arguments.Count < 2)
        {
            response = $"Usage: give <player> <roleId>";
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
        
        CustomRoleHandler.Give(target, role);

        response = $"Player '{target.Nickname}' received role '{role.Name}'.";
        return true;
    }

    public string Command { get; } = "giveroles";
    public string[] Aliases { get; } = { };
    public string Description { get; } = "Gives a custom role to a player.";
}