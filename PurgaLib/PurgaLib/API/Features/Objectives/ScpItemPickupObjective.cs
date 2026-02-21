using PurgaLib.API.Enums;
using Respawning.Objectives;

namespace PurgaLib.API.Features.Objectives
{
    using BaseObjective = Respawning.Objectives.ScpItemPickupObjective;
    
    public sealed class ScpItemPickupObjective : HumanObjective<PickupObjectiveFootprint>
    {
        public new BaseObjective Base { get; }

        public override ObjectiveType Type => ObjectiveType.ScpItemPickup;

        internal ScpItemPickupObjective(BaseObjective baseObjective)
            : base(baseObjective)
        {
            Base = baseObjective;
        }
        
        public void AddItem(Player target, Item item, Pickup pickup)
        {
            if (target == null || item == null || pickup == null) return;
            Base.OnItemAdded(target.ReferenceHub, item.Base, pickup.Base);
        }
        
                public void AddItem(Player target, Item item)
        {
            if (target == null || item == null) return;
            var pickup = item.Pickup;
            if (pickup != null)
                Base.OnItemAdded(target.ReferenceHub, item.Base, pickup);
        }
        
        public void AddItem(Player target, Pickup pickup)
        {
            if (target == null || pickup == null) return;
            var item = Item.Create(pickup.Type, target);
            Base.OnItemAdded(target.ReferenceHub, item.Base, pickup.Base);
        }
        
        public void AddItem(Player target, ItemType type)
        {
            if (target == null) return;
            AddItem(target, Item.Create(type, target));
        }
    }
}