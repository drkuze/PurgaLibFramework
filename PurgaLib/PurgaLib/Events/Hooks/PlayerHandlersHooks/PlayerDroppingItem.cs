namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using InventorySystem;
    using InventorySystem.Items;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;
    using System;


    [HarmonyPatch(typeof(InventoryExtensions), nameof(InventoryExtensions.ServerDropItem))]
    internal static class PlayerDroppingItem
    {
        private static bool Prefix(Inventory inv, ushort itemSerial)
        {
            try
            {
                if (inv == null) { return true; }

                if (inv._hub == null) { return true; }

                Player player = Player.Get(inv._hub);

                if (player == null) { return true; }

                if (player.IsHost) { return true; }

                if (inv.UserInventory == null) { return true; }

                if (inv.UserInventory.Items == null) { return true; }

                if (!inv.UserInventory.Items.TryGetValue(itemSerial, out ItemBase item)) { return true; }

                if (item == null) { return true; }

                ItemType itemType = item.ItemTypeId;

                var ev = new PlayerDroppingItemEventArgs(player, item, itemType);
                PlayerHandlers.InvokeSafely(ev);

                if (ev.IsAllowed) { return true; } else { return false; }
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] DroppingItem event error:\n{ex}");
                return true;
            }
        }
    }
}