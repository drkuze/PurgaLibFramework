using CommandSystem;
using PurgaLib.API.Features;
using PurgaLib.CustomItems.Handlers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace PurgaLib.CustomItems
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CustomItemCommand : ICommand
    {
        public string Command { get; } = "customitem";
        public string[] Aliases { get; } = { "ci" };
        public string Description { get; } = "CustomItem command (list, give).";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
        {
            response = string.Empty;

            if (arguments.Count == 0)
            {
                response = "Usage: customitem <list|give>";
                return false;
            }

            var args = arguments.ToArray();
            string subCommand = args[0].ToLower();

            switch (subCommand)
            {
                case "list":
                    return HandleList(out response);

                case "give":
                    return HandleGive(args, out response);

                default:
                    response = "Unknown subcommand. Usage: customitem <list|give>";
                    return false;
            }
        }

        private bool HandleList(out string response)
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

        private bool HandleGive(string[] args, out string response)
        {
            if (args.Length < 3)
            {
                response = "Usage: customitem give <player> <customItemId>";
                return false;
            }

            string targetName = args[1];
            string itemId = args[2];

            var target = Player.List.FirstOrDefault(p =>
                p.Nickname.Equals(targetName, StringComparison.OrdinalIgnoreCase));

            if (target == null)
            {
                response = $"Player '{targetName}' not found.";
                return false;
            }

            var custom = CustomItemHandler.Registered.FirstOrDefault(c =>
                c.Id.Equals(itemId, StringComparison.OrdinalIgnoreCase));

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
}
