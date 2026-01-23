using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round
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