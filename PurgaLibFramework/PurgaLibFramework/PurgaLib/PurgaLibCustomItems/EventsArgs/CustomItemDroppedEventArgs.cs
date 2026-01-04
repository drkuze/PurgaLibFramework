using LabApi.Features.Wrappers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.EventsArgs;

public class CustomItemDroppedEventArgs : System.EventArgs
{
    public Player Player { get; }
    public Item Item { get; }

    public CustomItemDroppedEventArgs(Player player, Item item)
    {
        Player = player;
        Item = item;
    }
}