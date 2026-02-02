using System.Collections.Generic;
using PlayerRoles;
using PurgaLib.API.Features;

namespace PurgaLib.CustomRoles
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
                OriginalRoles[player] = player.Role.Type;

            player.Role.Set(BaseRoleType);
        }

        public void OnRemove(Player player)
        {
            if (player == null) return;

            if (OriginalRoles.TryGetValue(player, out var original))
            {
                player.Role.Set(original);
                OriginalRoles.Remove(player);
            }
        }
    }
}
