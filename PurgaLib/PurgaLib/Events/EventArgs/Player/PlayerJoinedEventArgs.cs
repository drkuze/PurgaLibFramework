using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerJoinedEventArgs : IEventArgs
{
    public global::PurgaLib.API.Features.Player Player { get; }
    public PlayerJoinedEventArgs(global::PurgaLib.API.Features.Player player) => Player = player;
    public bool IsAllowed { get; set; }
}