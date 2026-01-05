namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerKickedEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Player { get; }
    public string Reason { get; }

    public PlayerKickedEventArgs(LabApi.Features.Wrappers.Player player, string reason)
    {
        Player = player;
        Reason = reason;
    }
}
