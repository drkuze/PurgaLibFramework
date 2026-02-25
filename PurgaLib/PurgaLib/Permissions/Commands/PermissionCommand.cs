using System;
using CommandSystem;
using PurgaLib.Permissions.Groups;

namespace PurgaLib.Permissions.Commands;

[CommandHandler(typeof(GameConsoleCommandHandler))]
public class PermissionCommand : ICommand
{
    public string Command { get; } = "purgaperm";
    public string[] Aliases { get; } = { "pperm" };
    public string Description { get; } = "Manages PurgaLib group permissions.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count < 3)
        {
            response = "Usage: purgaperm <add|remove> <group> <permission>";
            return false;
        }

        string action = arguments.At(0).ToLower();
        string groupName = arguments.At(1);
        string perm = arguments.At(2);
        
        if (!GroupsHandler.GroupDict.TryGetValue(groupName, out var group))
        {
            response = $"Group '{groupName}' does not exist in PurgaLib.";
            return false;
        }

        switch (action)
        {
            case "add":
                if (group.Permissions.Contains(perm))
                {
                    response = $"Group '{groupName}' already has permission '{perm}'.";
                    return false;
                }
                group.Permissions.Add(perm);
                Permissions.Save(); 
                response = $"Permission '{perm}' successfully added to group '{groupName}'.";
                return true;

            case "remove":
                if (group.Permissions.Remove(perm))
                {
                    Permissions.Save();
                    response = $"Permission '{perm}' successfully removed from group '{groupName}'.";
                    return true;
                }
                response = $"Group '{groupName}' does not have permission '{perm}'.";
                return false;

            default:
                response = "Invalid action. Use 'add' or 'remove'.";
                return false;
        }
    }
}