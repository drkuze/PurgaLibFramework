namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerSpawningEventArgs : System.EventArgs
{
    public PurgaLibAPI.Features.Player Player { get; }
    public bool IsAllowed { get; set; } = true;

    public PlayerSpawningEventArgs(PurgaLibAPI.Features.Player player) => Player = player;
}

public class PlayerSpawnedEventArgs : System.EventArgs
{
    public PurgaLibAPI.Features.Player Player { get; }
    public PlayerSpawnedEventArgs(PurgaLibAPI.Features.Player player) => Player = player;
}