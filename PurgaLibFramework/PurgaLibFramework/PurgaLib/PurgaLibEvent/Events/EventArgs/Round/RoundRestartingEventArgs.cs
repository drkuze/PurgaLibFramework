using System;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round
{
    public class RoundRestartingEventArgs : System.EventArgs
    {
        public bool IsAllowed { get; set; } = true;
        public string Reason { get; }

        public RoundRestartingEventArgs(string reason = "Unknown")
        {
            Reason = reason;
        }
    }
}