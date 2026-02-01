using PurgaLib.API.Features;


namespace PurgaLib.CustomItems.EventsArgs;

public class CustomItemPickedUpEventArgs : System.EventArgs
{
    public Player Player { get; }
    public Item Item { get; }

    public CustomItemPickedUpEventArgs(Player player, Item item)
    {
        Player = player;
        Item = item;
    }
}