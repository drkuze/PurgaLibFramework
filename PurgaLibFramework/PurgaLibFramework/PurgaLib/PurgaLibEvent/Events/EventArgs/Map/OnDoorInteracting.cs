using LabApi.Features.Wrappers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map;

public class DoorInteractingEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Player { get; }
    public string DoorName { get; }
    public bool IsAllowed { get; set; } = true;

    public DoorInteractingEventArgs(LabApi.Features.Wrappers.Player player, string doorName)
    {
        Player = player;
        DoorName = doorName;
    }
}