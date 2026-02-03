using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Server
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