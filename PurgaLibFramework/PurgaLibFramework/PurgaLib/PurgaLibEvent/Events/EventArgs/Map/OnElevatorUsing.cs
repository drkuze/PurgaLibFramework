using LabApi.Features.Wrappers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map;

public class ElevatorUsingEventArgs : System.EventArgs
{
    public PurgaLibAPI.Features.Player Player { get; }
    public Elevator Elevator { get; }
    public bool IsAllowed { get; set; } = true;

    public ElevatorUsingEventArgs(PurgaLibAPI.Features.Player player, Elevator elevator)
    {
        Player = player;
        Elevator = elevator;
    }
}