using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.EventsArgs;

public class CustomItemUsedEventArgs : System.EventArgs
{
    public Player Player { get; }
    public Item Item { get; }

    public CustomItemUsedEventArgs(Player player, Item item)
    {
        Player = player;
        Item = item;
    }
}