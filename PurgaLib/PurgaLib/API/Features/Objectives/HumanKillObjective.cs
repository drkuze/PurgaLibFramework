using PlayerRoles;
using PurgaLib.API.Enums;

namespace PurgaLib.API.Features.Objectives
{
    using BaseObjective = Respawning.Objectives.HumanKillObjective;

    public sealed class HumanKillObjective : Objective
    {
        public new BaseObjective Base { get; }

        public override ObjectiveType Type => ObjectiveType.HumanKill;

        internal HumanKillObjective(BaseObjective baseObjective)
            : base(baseObjective)
        {
            Base = baseObjective;
        }
        
        public bool IsValidEnemy(RoleTypeId targetRole, Player player)
            => Base.IsValidEnemy(targetRole, player.ReferenceHub);
        
        public bool IsValidEnemy(Player target, Player player)
            => IsValidEnemy(target.Role.Type, player);
        
        public void Kill(DamageHandler.DamageHandler damageHandler)
        {
            if (damageHandler?.Attacker == null) return;
            Base.OnKill(damageHandler.Victim, damageHandler.Base);
        }
        
        public void Kill(Player target, Player attacker, DamageType type = DamageType.None)
        {
            if (target == null || attacker == null) return;
            Kill(new DamageHandler.DamageHandler(target, attacker, -1, type, string.Empty));
        }
    }
}