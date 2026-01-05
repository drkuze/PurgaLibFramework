using System;
using System.Collections.Generic;
using InventorySystem.Items;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.EventsArgs;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.Handlers
{
    public static class CustomItemHandler
    {
        public static readonly List<CustomItem> Registered = new();
        internal static readonly Dictionary<Item, CustomItem> ItemMap = new();

        private static EventHandler<CustomItemUsedEventArgs> _used;
        private static EventHandler<CustomItemDroppedEventArgs> _dropped;
        private static EventHandler<CustomItemPickedUpEventArgs> _pickedUp;

        public static event EventHandler<CustomItemUsedEventArgs> Used
        {
            add { bool wasEmpty = _used == null; _used += value; if (wasEmpty) RegisterEvents(); }
            remove => _used -= value;
        }

        public static event EventHandler<CustomItemDroppedEventArgs> Dropped
        {
            add { bool wasEmpty = _dropped == null; _dropped += value; if (wasEmpty) RegisterEvents(); }
            remove => _dropped -= value;
        }

        public static event EventHandler<CustomItemPickedUpEventArgs> PickedUp
        {
            add { bool wasEmpty = _pickedUp == null; _pickedUp += value; if (wasEmpty) RegisterEvents(); }
            remove => _pickedUp -= value;
        }

        public static void Register(CustomItem customItem)
        {
            if (customItem == null || Registered.Contains(customItem)) return;
            Registered.Add(customItem);
        }

        public static void Give(Player player, CustomItem custom)
        {
            var item = player.AddItem(custom.BaseType, ItemAddReason.Undefined);
            
            ItemMap[item] = custom;
        }

        internal static bool TryGet(Item item, out CustomItem custom)
        {
            custom = null;
            if (item == null) return false;
            return ItemMap.TryGetValue(item, out custom);
        }

        internal static void RemoveItemFromMap(Item item)
        {
            if (item != null && ItemMap.ContainsKey(item))
                ItemMap.Remove(item);
        }

        public static void RegisterEvents()
        {
            PlayerEvents.UsedItem += ev =>
            {
                var item = ev.Item ?? ev.Player.CurrentItem;
                if (!TryGet(item, out var custom)) return;

                custom.OnUse(ev.Player);
                _used?.Invoke(null, new CustomItemUsedEventArgs(ev.Player, item));
            };
            
            PlayerEvents.DroppedItem += ev =>
            {
                var item = ev.Player.CurrentItem;
                if (!TryGet(item, out var custom)) return;

                custom.OnDrop(ev.Player);
                _dropped?.Invoke(null, new CustomItemDroppedEventArgs(ev.Player, item));

                RemoveItemFromMap(item);
            };
            
            PlayerEvents.PickedUpItem += ev =>
            {
                var item = ev.Item ?? ev.Player.CurrentItem;
                if (!TryGet(item, out var custom)) return;

                custom.OnPickup(ev.Player);
                _pickedUp?.Invoke(null, new CustomItemPickedUpEventArgs(ev.Player, item));
            };

            Log.Success("[PurgaLib] CustomItemHandler events registered.");
        }
    }
}
