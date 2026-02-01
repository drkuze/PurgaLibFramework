using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerKickedEventArgs : IEventArgs
{
    public global::PurgaLib.API.Features.Player Player { get; }
    public string Reason { get; }

    public PlayerKickedEventArgs(global::PurgaLib.API.Features.Player player, string reason)
    {
        Player = player;
        Reason = reason;
    }

    public bool IsAllowed { get; set; }
}
