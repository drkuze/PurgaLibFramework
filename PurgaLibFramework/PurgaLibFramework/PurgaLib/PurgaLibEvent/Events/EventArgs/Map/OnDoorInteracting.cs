using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;

namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Map
{
    public class DoorInteractingEventArgs : System.EventArgs
    {
        public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
        public Door Door { get; }
        public bool IsAllowed { get; set; } = true;

        public DoorInteractingEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player, Door door)
        {
            Player = player;
            Door = door;
        }
    }
}