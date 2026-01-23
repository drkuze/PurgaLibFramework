using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerDiedEventArgs : IEventArgs
    {
        public global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
        public global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Killer { get; }
        public int DamageType { get; }

        public PlayerDiedEventArgs(global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player, global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player killer, int damageType)
        {
            Player = player;
            Killer = killer;
            DamageType = damageType;
        }

        public bool IsAllowed { get; set; }
    }
}