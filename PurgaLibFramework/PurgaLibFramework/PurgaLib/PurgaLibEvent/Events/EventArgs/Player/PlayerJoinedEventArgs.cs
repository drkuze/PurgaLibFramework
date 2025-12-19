namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerJoinedEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Player { get; }
    public PlayerJoinedEventArgs(LabApi.Features.Wrappers.Player player) => Player = player;
}