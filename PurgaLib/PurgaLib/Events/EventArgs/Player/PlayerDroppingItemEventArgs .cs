using InventorySystem.Items;
using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerDroppingItemEventArgs : IEventArgs
{
    public global::PurgaLib.API.Features.Player Player { get; }
    public ItemBase Item { get; }
    public ItemType ItemType { get; }
    public bool IsAllowed { get; set; } = true;

    public PlayerDroppingItemEventArgs(global::PurgaLib.API.Features.Player player, ItemBase item, ItemType itemType)
    {
        Player = player;
        Item = item;
        ItemType = itemType;
    }
}