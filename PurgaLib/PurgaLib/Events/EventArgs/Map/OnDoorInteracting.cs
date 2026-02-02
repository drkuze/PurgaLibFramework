using PurgaLib.API.Features;
using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Map
{
    public class DoorInteractingEventArgs : IEventArgs
    {
        public API.Features.Player Player { get; }
        public Door Door { get; }
        public bool IsAllowed { get; set; } = true;

        public DoorInteractingEventArgs(API.Features.Player player, Door door)
        {
            Player = player;
            Door = door;
        }
    }
}