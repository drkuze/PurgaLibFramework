namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerDyingEventArgs : System.EventArgs
    {
        public LabApi.Features.Wrappers.Player Player { get; }
        public LabApi.Features.Wrappers.Player Killer { get; }
        public string Reason { get; }
        public bool IsAllowed { get; set; } = true;

        public PlayerDyingEventArgs(LabApi.Features.Wrappers.Player player, LabApi.Features.Wrappers.Player killer, string reason)
        {
            Player = player;
            Killer = killer;
            Reason = reason;
        }
    }
}