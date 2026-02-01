using CommandSystem;
using PurgaLib.API.Features;
using PurgaLib.CustomRoles.Handlers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace PurgaLib.CustomRoles
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CustomRolesCommand : ICommand
    {
        public string Command { get; } = "customroles";
        public string[] Aliases { get; } = { "cr" };
        public string Description { get; } = "CustomRoles command (list, give, remove).";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
        {
            response = string.Empty;

            if (arguments.Count == 0)
            {
                response = "Usage: customroles <list|give|remove>";
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

                case "remove":
                    return HandleRemove(args, out response);

                default:
                    response = "Unknown subcommand. Usage: customroles <list|give|remove>";
                    return false;
            }
        }

        private bool HandleList(out string response)
        {
            if (!CustomRoleHandler.Registered.Any())
            {
                response = "No custom roles are registered.";
                return true;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Registered Custom Roles:");

            foreach (var role in CustomRoleHandler.Registered)
                sb.AppendLine($"- {role.Id} | {role.Name}");

            response = sb.ToString();
            return true;
        }

        private bool HandleGive(string[] args, out string response)
        {
            if (args.Length < 3)
            {
                response = "Usage: customroles give <player> <roleId>";
                return false;
            }

            string targetName = args[1];
            string roleId = args[2];

            Player target = Player.List.FirstOrDefault(p =>
                p.Nickname.Equals(targetName, StringComparison.OrdinalIgnoreCase));

            if (target == null)
            {
                response = $"Player '{targetName}' not found.";
                return false;
            }

            CustomRole role = CustomRoleHandler.Registered.FirstOrDefault(r =>
                r.Id.Equals(roleId, StringComparison.OrdinalIgnoreCase));

            if (role == null)
            {
                response = $"Role '{roleId}' does not exist.";
                return false;
            }

            CustomRoleHandler.Give(target, role);
            response = $"Player '{target.Nickname}' received role '{role.Name}'.";
            return true;
        }

        private bool HandleRemove(string[] args, out string response)
        {
            if (args.Length < 3)
            {
                response = "Usage: customroles remove <player> <roleId>";
                return false;
            }

            string targetName = args[1];
            string roleId = args[2];

            Player target = Player.List.FirstOrDefault(p =>
                p.Nickname.Equals(targetName, StringComparison.OrdinalIgnoreCase));

            if (target == null)
            {
                response = $"Player '{targetName}' not found.";
                return false;
            }

            CustomRole role = CustomRoleHandler.Registered.FirstOrDefault(r =>
                r.Id.Equals(roleId, StringComparison.OrdinalIgnoreCase));

            if (role == null)
            {
                response = $"Role '{roleId}' does not exist.";
                return false;
            }

            CustomRoleHandler.Remove(target, role);
            response = $"Player '{target.Nickname}' had role '{role.Name}' removed.";
            return true;
        }
    }
}
