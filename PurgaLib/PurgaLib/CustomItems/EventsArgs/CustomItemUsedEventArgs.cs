using PurgaLib.API.Features;

namespace PurgaLib.CustomItems.EventsArgs;

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