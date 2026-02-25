using System;
using System.Collections.Generic;
using CommandSystem;
using PurgaLib.Permissions.Groups;

namespace PurgaLib.Permissions.Commands;

[CommandHandler(typeof(GameConsoleCommandHandler))]
public class GroupCommand : ICommand
{
    public string Command { get; } = "purgagroup";
    public string[] Aliases { get; } = { "pgroup" };
    public string Description { get; } = "Manages PurgaLib permission groups.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count < 1)
        {
            response = "Usage: purgagroup <add|remove|list> <groupName>";
            return false;
        }

        string action = arguments.At(0).ToLower();

        switch (action)
        {
            case "list":
                response = "Registered PurgaLib Groups:\n- " + string.Join("\n- ", GroupsHandler.GroupDict.Keys);
                return true;

            case "add":
                if (arguments.Count < 2)
                {
                    response = "Usage: purgagroup add <groupName>";
                    return false;
                }

                string nameToAdd = arguments.At(1);
                if (GroupsHandler.GroupDict.ContainsKey(nameToAdd))
                {
                    response = $"Group '{nameToAdd}' already exists.";
                    return false;
                }

                GroupsHandler.GroupDict.Add(nameToAdd, new Group { IsDefault = false, Permissions = new List<string>() });
                Permissions.Save();
                response = $"Group '{nameToAdd}' has been successfully created.";
                return true;

            case "remove":
                if (arguments.Count < 2)
                {
                    response = "Usage: purgagroup remove <groupName>";
                    return false;
                }

                string nameToRem = arguments.At(1);
                if (GroupsHandler.GroupDict.Remove(nameToRem))
                {
                    Permissions.Save();
                    response = $"Group '{nameToRem}' has been successfully removed.";
                    return true;
                }

                response = $"Group '{nameToRem}' not found.";
                return false;

            default:
                response = "Invalid action. Use 'add', 'remove', or 'list'.";
                return false;
        }
    }
}