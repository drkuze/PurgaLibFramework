namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerLeftEventArgs : System.EventArgs
{
    public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
    public PlayerLeftEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player) => Player = player;
}