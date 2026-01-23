using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round
{
    public class RoundRestartingEventArgs : IEventArgs
    {
        public bool IsAllowed { get; set; } = true;
        public string Reason { get; }

        public RoundRestartingEventArgs(string reason = "Unknown")
        {
            Reason = reason;
        }
    }
}