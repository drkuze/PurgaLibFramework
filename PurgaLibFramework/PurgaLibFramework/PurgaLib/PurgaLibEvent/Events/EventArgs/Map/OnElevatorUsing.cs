using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;

namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Map;

public class ElevatorUsingEventArgs : System.EventArgs
{
    public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
    public Elevator Elevator { get; }
    public bool IsAllowed { get; set; } = true;

    public ElevatorUsingEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player, Elevator elevator)
    {
        Player = player;
        Elevator = elevator;
    }
}