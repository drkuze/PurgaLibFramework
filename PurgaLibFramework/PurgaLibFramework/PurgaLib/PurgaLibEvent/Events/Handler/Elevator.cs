using PurgaLibEvents.PurgaLibEvent.Attribute;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Map;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class ElevatorHandler
    {
        public static Event<ElevatorUsingEventArgs> Interacting;
        internal static void OnInteracting(ElevatorUsingEventArgs ev) => Interacting?.Invoke(ev);
    }
}
