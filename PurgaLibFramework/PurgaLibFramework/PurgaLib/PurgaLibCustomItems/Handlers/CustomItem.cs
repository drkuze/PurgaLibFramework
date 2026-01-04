using System;
using LabApi.Events.Handlers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.Events;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.Handlers
{
    public static class CustomItemHandler
    {
        private static EventHandler<ItemUsingEventArgs> _using;

        public static event EventHandler<ItemUsingEventArgs> Using
        {
            add
            {
                bool wasEmpty = _using == null;
                _using += value;
                if (wasEmpty)
                    RegisterLabApi();
            }
            remove => _using -= value;
        }

        private static void OnUsing(ItemUsingEventArgs args) => _using?.Invoke(null, args);

        public static void RegisterLabApi()
        {
            PlayerEvents.UsingItem += ev =>
            {
                string itemId = ev.Item.Base.name;
                var customItem = CustomItemsDatabase.GetItemById(itemId);
                if (customItem == null)
                    return;

                var args = new ItemUsingEventArgs(ev.Player, itemId);
                OnUsing(args);
            };
        }
    }
}