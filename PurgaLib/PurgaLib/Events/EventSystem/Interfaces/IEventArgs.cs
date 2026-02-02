    namespace PurgaLib.Events.EventSystem.Interfaces;

    public interface IEventArgs
    {
        bool IsAllowed { get; set; }
    }