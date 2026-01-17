namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerKickedEventArgs : System.EventArgs
{
    public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
    public string Reason { get; }

    public PlayerKickedEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player, string reason)
    {
        Player = player;
        Reason = reason;
    }
}
