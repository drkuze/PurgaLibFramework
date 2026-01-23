using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerJoinedEventArgs : IEventArgs
{
    public global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
    public PlayerJoinedEventArgs(global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player) => Player = player;
    public bool IsAllowed { get; set; }
}