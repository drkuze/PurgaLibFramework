using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CommandSystem;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.Handlers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.Command;

[CommandHandler(typeof(GameConsoleCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class CustomItemList : ICommand
{
    public string Command { get; } = "listitems";
    public string[] Aliases { get; } = { };
    public string Description { get; } = "Lists all registered custom items.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (CustomItemHandler.Registered.Count == 0)
        {
            response = "No custom items registered.";
            return true;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Registered Custom Items:");

        foreach (var item in CustomItemHandler.Registered)
            sb.AppendLine($"- {item.Id} | {item.Name} | {item.Description}");

        response = sb.ToString();
        return true;
    }
}