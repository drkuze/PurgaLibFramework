using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerSpawningEventArgs : IEventArgs
{
    public global::PurgaLib.API.Features.Player Player { get; }
    public bool IsAllowed { get; set; } = true;

    public PlayerSpawningEventArgs(global::PurgaLib.API.Features.Player player) => Player = player;
}

public class PlayerSpawnedEventArgs : System.EventArgs
{
    public global::PurgaLib.API.Features.Player Player { get; }
    public PlayerSpawnedEventArgs(global::PurgaLib.API.Features.Player player) => Player = player;
}