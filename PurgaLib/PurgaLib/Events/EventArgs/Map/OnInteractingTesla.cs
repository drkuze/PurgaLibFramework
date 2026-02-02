using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Map
{
    public class OnInteractingTeslaEventArgs : IEventArgs
    {
        public API.Features.Player Player { get; }
        public TeslaGate TeslaGate { get; }
        public bool IsAllowed { get; set; }

        public OnInteractingTeslaEventArgs(API.Features.Player player,TeslaGate teslaGate, bool isAllowed = true)
        {
            Player = player ?? throw new System.ArgumentNullException(nameof(player));
            IsAllowed = isAllowed;
            TeslaGate = teslaGate;
        }
    }
}