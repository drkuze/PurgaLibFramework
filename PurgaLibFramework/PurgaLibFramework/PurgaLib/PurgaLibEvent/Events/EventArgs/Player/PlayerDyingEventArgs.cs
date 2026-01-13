namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerDyingEventArgs : System.EventArgs
    {
        public PurgaLibAPI.Features.Player Player { get; }
        public PurgaLibAPI.Features.Player Killer { get; }
        public string Reason { get; }
        public bool IsAllowed { get; set; } = true;

        public PlayerDyingEventArgs(PurgaLibAPI.Features.Player player, PurgaLibAPI.Features.Player killer, string reason)
        {
            Player = player;
            Killer = killer;
            Reason = reason;
        }
    }
}