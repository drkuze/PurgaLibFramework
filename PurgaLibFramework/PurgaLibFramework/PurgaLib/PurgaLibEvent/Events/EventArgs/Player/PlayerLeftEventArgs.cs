namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerLeftEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Player { get; }
    public PlayerLeftEventArgs(LabApi.Features.Wrappers.Player player) => Player = player;
}