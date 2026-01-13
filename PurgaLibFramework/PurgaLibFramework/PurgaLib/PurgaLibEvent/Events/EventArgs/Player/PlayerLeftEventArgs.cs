namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerLeftEventArgs : System.EventArgs
{
    public PurgaLibAPI.Features.Player Player { get; }
    public PlayerLeftEventArgs(PurgaLibAPI.Features.Player player) => Player = player;
}