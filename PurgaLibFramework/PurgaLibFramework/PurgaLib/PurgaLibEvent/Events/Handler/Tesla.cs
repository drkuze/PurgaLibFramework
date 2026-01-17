using PurgaLibEvents.PurgaLibEvent.Attribute;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Map;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class TeslaHandler
    {
        public static Event<OnInteractingTeslaEventArgs> InteractingTesla;
        internal static void OnInteractingTesla(OnInteractingTeslaEventArgs ev) => InteractingTesla?.Invoke(ev);
    }
}
