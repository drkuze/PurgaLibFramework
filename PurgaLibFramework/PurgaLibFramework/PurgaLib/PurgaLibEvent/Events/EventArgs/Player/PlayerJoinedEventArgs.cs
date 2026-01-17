namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerJoinedEventArgs : System.EventArgs
{
    public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
    public PlayerJoinedEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player) => Player = player;
}