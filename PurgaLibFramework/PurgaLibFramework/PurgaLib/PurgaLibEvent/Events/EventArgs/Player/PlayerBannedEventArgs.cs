namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerBannedEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Player { get; }
    public string Reason { get; }
    public long Duration { get; }

    public PlayerBannedEventArgs(LabApi.Features.Wrappers.Player player, string reason, long duration)
    {
        Player = player;
        Duration = duration;
        Reason = reason;
    }
}