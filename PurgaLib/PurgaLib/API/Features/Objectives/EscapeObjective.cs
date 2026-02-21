using PlayerRoles;
using PurgaLib.API.Enums;

namespace PurgaLib.API.Features.Objectives
{
    using BaseObjective = Respawning.Objectives.EscapeObjective;

    public sealed class EscapeObjective : Objective
    {
        public new BaseObjective Base { get; }

        public override ObjectiveType Type => ObjectiveType.Escape;


        internal EscapeObjective(BaseObjective baseObjective)
            : base(baseObjective)
        {
            Base = baseObjective;
        }

        
        public void ForceEscape(Player player, RoleTypeId newRole)
        {
            if (player == null)
                return;

            Base.OnServerRoleSet(
                player.ReferenceHub,
                newRole,
                RoleChangeReason.Escaped
            );
        }

        
        public void EscapeAsDClass(Player player)
            => ForceEscape(player, RoleTypeId.ClassD);

        public void EscapeAsScientist(Player player)
            => ForceEscape(player, RoleTypeId.Scientist);

        
        public static void EscapePlayer(Player player, RoleTypeId newRole)
        {
            if (player == null)
                return;
            player.Role.Set(newRole);
        }
    }
}