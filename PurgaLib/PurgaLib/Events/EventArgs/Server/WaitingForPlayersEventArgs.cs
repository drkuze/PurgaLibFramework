using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Server;

public class WaitingForPlayersEventArgs : IEventArgs
{
    public bool IsAllowed { get; set; } = true;
}