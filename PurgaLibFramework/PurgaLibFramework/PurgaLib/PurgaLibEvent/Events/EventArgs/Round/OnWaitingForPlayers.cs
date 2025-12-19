namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round
{
    public class WaitingForPlayersEventArgs : System.EventArgs
    {
        public bool IsAllowed { get; set; } = true;
    }
}