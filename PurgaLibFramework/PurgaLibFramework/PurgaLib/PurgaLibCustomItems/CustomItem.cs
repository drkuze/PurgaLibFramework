using LabApi.Features.Wrappers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems
{
    public abstract class CustomItem
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract ItemType BaseType { get; }
        
        public virtual void OnUse(Player player) { }
        public virtual void OnDrop(Player player) { }
        public virtual void OnPickup(Player player) { }
    }
}