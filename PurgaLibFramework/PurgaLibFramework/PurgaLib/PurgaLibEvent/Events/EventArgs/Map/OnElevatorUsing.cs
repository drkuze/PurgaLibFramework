namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map;

public class ElevatorUsingEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Player { get; }
    public string Elevator { get; }

    public ElevatorUsingEventArgs(LabApi.Features.Wrappers.Player player, string elevator)
    {
        Player = player;
        Elevator = elevator;
    }
}