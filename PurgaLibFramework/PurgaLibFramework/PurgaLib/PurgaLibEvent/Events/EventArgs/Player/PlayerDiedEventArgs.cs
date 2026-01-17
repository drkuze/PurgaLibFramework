namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerDiedEventArgs : System.EventArgs
    {
        public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
        public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Killer { get; }
        public int DamageType { get; }

        public PlayerDiedEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player, PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player killer, int damageType)
        {
            Player = player;
            Killer = killer;
            DamageType = damageType;
        }
    }
}