using System.Collections.Generic;
using System.Linq;
using PlayerRoles;
using PurgaLib.API.Enums;
using Respawning;
using Respawning.Objectives;

namespace PurgaLib.API.Features.Objectives
{
    public class Objective
    {
        private static readonly Dictionary<FactionObjectiveBase, Objective> Cache = new();

        public static IReadOnlyCollection<Objective> List => Cache.Values;
        
        public FactionObjectiveBase Base { get; }
        
        public virtual ObjectiveType Type => ObjectiveType.None;

        protected Objective(FactionObjectiveBase baseObjective)
        {
            Base = baseObjective;
            Cache[baseObjective] = this;
        }
        
        public static Objective Get(ObjectiveType type)
        {
            FactionObjectiveBase obj = type switch
            {
                ObjectiveType.Escape =>
                    FactionInfluenceManager.Objectives.OfType<Respawning.Objectives.EscapeObjective>().FirstOrDefault(),

                ObjectiveType.GeneratorActivation =>
                    FactionInfluenceManager.Objectives.OfType<Respawning.Objectives.GeneratorActivatedObjective>().FirstOrDefault(),

                ObjectiveType.HumanKill =>
                    FactionInfluenceManager.Objectives.OfType<Respawning.Objectives.HumanKillObjective>().FirstOrDefault(),

                ObjectiveType.HumanDamage =>
                    FactionInfluenceManager.Objectives.OfType<Respawning.Objectives.HumanDamageObjective>().FirstOrDefault(),

                ObjectiveType.ScpItemPickup =>
                    FactionInfluenceManager.Objectives.OfType<Respawning.Objectives.ScpItemPickupObjective>().FirstOrDefault(),

                _ => null
            };

            return obj == null ? null : Wrap(obj);
        }
        
        public static Objective Wrap(FactionObjectiveBase baseObjective)
        {
            if (baseObjective == null)
                return null;

            if (Cache.TryGetValue(baseObjective, out var existing))
                return existing;

            return new Objective(baseObjective);
        }

        // Operazioni base
        public void Achieve() => Base.ServerSendUpdate();
        public void Grant(Faction faction, float amount) => Base.GrantInfluence(faction, amount);
        public void Reduce(Faction faction, float seconds) => Base.ReduceTimer(faction, seconds);

        public bool IsValid(Faction faction) => Base.IsValidFaction(faction);
        public bool IsValid(Player player) => Base.IsValidFaction(player.ReferenceHub);
    }
}