using PurgaLib.API.Enums;

namespace PurgaLib.API.Features.Objectives
{
    using BaseObjective = Respawning.Objectives.HumanDamageObjective;

    public sealed class HumanDamageObjective : Objective
    {
        public new BaseObjective Base { get; }

        public override ObjectiveType Type => ObjectiveType.HumanDamage;

        internal HumanDamageObjective(BaseObjective baseObjective)
            : base(baseObjective)
        {
            Base = baseObjective;
        }
        
        public void Damage(Player attacker, Player target, float amount, DamageType type = DamageType.None)
        {
            if (attacker == null || target == null) return;

            Damage(new DamageHandler.DamageHandler(target, attacker, amount, type, string.Empty));
        }
        
        public void Damage(DamageHandler.DamageHandler damageHandler)
        {
            if (damageHandler?.Attacker == null) return;
            
            Base.OnPlayerDamaged(damageHandler.Attacker, damageHandler.Base);
        }
    }
}