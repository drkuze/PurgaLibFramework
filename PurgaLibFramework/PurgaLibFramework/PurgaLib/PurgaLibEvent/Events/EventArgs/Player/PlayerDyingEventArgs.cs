namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerDyingEventArgs : System.EventArgs
    {
        public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
        public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Killer { get; }
        public string Reason { get; }
        public bool IsAllowed { get; set; } = true;

        public PlayerDyingEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player, PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player killer, string reason)
        {
            Player = player;
            Killer = killer;
            Reason = reason;
        }
    }
}