using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map
{
    public class OnInteractingTeslaEventArgs : IEventArgs
    {
        public global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
        public TeslaGate TeslaGate { get; }
        public bool IsAllowed { get; set; }

        public OnInteractingTeslaEventArgs(global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player,TeslaGate teslaGate, bool isAllowed = true)
        {
            Player = player ?? throw new System.ArgumentNullException(nameof(player));
            IsAllowed = isAllowed;
            TeslaGate = teslaGate;
        }
    }
}