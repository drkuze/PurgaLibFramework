using System.Collections.Generic;
using PlayerRoles;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomRoles
{
    public class CustomRole
    {
        public string Id { get; }
        public string Name { get; }
        public string Prefix { get; }
        public string Color { get; }
        public RoleTypeId BaseRoleType { get; }

        private static readonly Dictionary<Player, RoleTypeId> OriginalRoles = new();

        public CustomRole(string id, string name, string prefix, string color, RoleTypeId baseRoleType)
        {
            Id = id;
            Name = name;
            Prefix = prefix;
            Color = color;
            BaseRoleType = baseRoleType;
        }

        public void OnAssign(Player player)
        {
            if (player == null) return;

            if (!OriginalRoles.ContainsKey(player))
                OriginalRoles[player] = player.Role;

            player.SetRole(BaseRoleType);
        }

        public void OnRemove(Player player)
        {
            if (player == null) return;

            if (OriginalRoles.TryGetValue(player, out var original))
            {
                player.SetRole(original);
                OriginalRoles.Remove(player);
            }
        }
    }
}
