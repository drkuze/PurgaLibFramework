using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player
{
    public class PlayerDyingEventArgs : IEventArgs
    {
        public global::PurgaLib.API.Features.Player Player { get; }
        public global::PurgaLib.API.Features.Player Killer { get; }
        public string Reason { get; }
        public bool IsAllowed { get; set; } = true;

        public PlayerDyingEventArgs(global::PurgaLib.API.Features.Player player, API.Features.Player killer, string reason)
        {
            Player = player;
            Killer = killer;
            Reason = reason;
        }
    }
}