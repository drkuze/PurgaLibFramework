using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Server
{
    public class RoundStartingEventArgs : IEventArgs
    {
        public bool IsAllowed { get; set; }
    }
    public class RoundStartedEventArgs : IEventArgs
    {
        public bool IsAllowed { get; set; }
    }
}