using System;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.Events
{
    public class ItemUsingEventArgs : System.EventArgs
    {
        public LabApi.Features.Wrappers.Player Player { get; }
        public string ItemId { get; }
        public bool IsAllowed { get; set; } = true;

        public ItemUsingEventArgs(LabApi.Features.Wrappers.Player player, string itemId)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        }
    }
}