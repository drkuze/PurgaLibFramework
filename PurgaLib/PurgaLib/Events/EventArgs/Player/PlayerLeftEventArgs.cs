using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerLeftEventArgs : IEventArgs
{
    public global::PurgaLib.API.Features.Player Player { get; }
    public PlayerLeftEventArgs(global::PurgaLib.API.Features.Player player) => Player = player;
    public bool IsAllowed { get; set; }
}