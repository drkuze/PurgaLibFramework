namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerDiedEventArgs : System.EventArgs
    {
        public PurgaLibAPI.Features.Player Player { get; }
        public PurgaLibAPI.Features.Player Killer { get; }
        public int DamageType { get; }

        public PlayerDiedEventArgs(PurgaLibAPI.Features.Player player, PurgaLibAPI.Features.Player killer, int damageType)
        {
            Player = player;
            Killer = killer;
            DamageType = damageType;
        }
    }
}