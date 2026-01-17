using PurgaLibEvents.PurgaLibEvent.Attribute;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Map;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class DoorHandler
    {
        public static Event<DoorInteractingEventArgs> Interacting;
        
        internal static void OnInteracting(DoorInteractingEventArgs ev) => Interacting?.Invoke(ev);
    }
}
