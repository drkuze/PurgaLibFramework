using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player
{
    public class PlayerDiedEventArgs : IEventArgs
    {
        public global::PurgaLib.API.Features.Player Player { get; }
        public global::PurgaLib.API.Features.Player Killer { get; }
        public int DamageType { get; }

        public PlayerDiedEventArgs(global::PurgaLib.API.Features.Player player, API.Features.Player killer, int damageType)
        {
            Player = player;
            Killer = killer;
            DamageType = damageType;
        }

        public bool IsAllowed { get; set; }
    }
}