using MEC;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;

public class Door
{
    internal LabApi.Features.Wrappers.Door Base { get; }
    internal Door(LabApi.Features.Wrappers.Door door)
    {
        Base = door;
    }
    public static Door Get(LabApi.Features.Wrappers.Door door)
        => door == null ? null : new Door(door);
    
    public static void LockAll(float duration)
    {
        foreach (var door in LabApi.Features.Wrappers.Door.List)
        {
            var d = door; 
            d.IsLocked = true;

            Timing.CallDelayed(duration, () => { d.IsLocked = false; });
        }
    }
}