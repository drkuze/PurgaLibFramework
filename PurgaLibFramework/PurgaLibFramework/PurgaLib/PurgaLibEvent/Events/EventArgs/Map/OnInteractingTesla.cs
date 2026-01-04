namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map
{
    public class OnInteractingTeslaEventArgs : System.EventArgs
    {
        public LabApi.Features.Wrappers.Player Player { get; }
        public bool IsEnabled { get; set; }

        public OnInteractingTeslaEventArgs(LabApi.Features.Wrappers.Player player, bool isEnabled = true)
        {
            Player = player ?? throw new System.ArgumentNullException(nameof(player));
            IsEnabled = isEnabled;
        }
    }
}