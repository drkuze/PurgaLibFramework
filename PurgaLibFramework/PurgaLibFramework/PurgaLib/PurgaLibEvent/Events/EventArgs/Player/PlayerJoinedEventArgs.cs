namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerJoinedEventArgs : System.EventArgs
{
    public PurgaLibAPI.Features.Player Player { get; }
    public PlayerJoinedEventArgs(PurgaLibAPI.Features.Player player) => Player = player;
}