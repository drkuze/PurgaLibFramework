using PurgaLib.API.Features;
using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Map;

public class ElevatorUsingEventArgs : IEventArgs
{
    public API.Features.Player Player { get; }
    public Elevator Elevator { get; }
    public bool IsAllowed { get; set; } = true;

    public ElevatorUsingEventArgs(API.Features.Player player, Elevator elevator)
    {
        Player = player;
        Elevator = elevator;
    }
}