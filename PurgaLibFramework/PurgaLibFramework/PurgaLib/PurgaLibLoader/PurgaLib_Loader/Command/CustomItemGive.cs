using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommandSystem;
using LabApi.Features.Wrappers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.Handlers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.Command;

[CommandHandler(typeof(GameConsoleCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class CustomItemGive : ICommand
{
    public string Command { get; } = "give";
    public string[] Aliases { get; } = { };
    public string Description { get; } = "Gives a custom item to a player.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        response = string.Empty;

        if (arguments.Count < 2)
        {
            response = "Usage: give <player> <customItemId>";
            return false;
        }

        string targetName = arguments[0];
        string itemId = arguments[1];

        var target = Player.List.FirstOrDefault(p => p.Nickname.Equals(targetName, StringComparison.OrdinalIgnoreCase));
        if (target == null)
        {
            response = $"Player '{targetName}' not found.";
            return false;
        }

        var custom = CustomItemHandler.Registered.FirstOrDefault(c => c.Id.Equals(itemId, StringComparison.OrdinalIgnoreCase));
        if (custom == null)
        {
            response = $"Custom item '{itemId}' does not exist.";
            return false;
        }

        CustomItemHandler.Give(target, custom);
        response = $"Player '{target.Nickname}' received item '{custom.Name}'.";
        return true;
    }
}