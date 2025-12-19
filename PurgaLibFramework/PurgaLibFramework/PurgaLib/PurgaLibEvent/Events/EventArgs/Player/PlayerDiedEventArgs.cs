namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerDiedEventArgs : System.EventArgs
    {
        public object Player { get; }
        public object Killer { get; }
        public string DamageType { get; }

        public PlayerDiedEventArgs(object player, object killer, string damageType)
        {
            Player = player;
            Killer = killer;
            DamageType = damageType;
        }
    }
}