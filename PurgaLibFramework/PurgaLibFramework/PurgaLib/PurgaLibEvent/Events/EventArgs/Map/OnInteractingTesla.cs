namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Map
{
    public class OnInteractingTeslaEventArgs : System.EventArgs
    {
        public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
        public bool IsAllowed { get; set; }

        public OnInteractingTeslaEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player, bool isAllowed = true)
        {
            Player = player ?? throw new System.ArgumentNullException(nameof(player));
            IsAllowed = isAllowed;
        }
    }
}