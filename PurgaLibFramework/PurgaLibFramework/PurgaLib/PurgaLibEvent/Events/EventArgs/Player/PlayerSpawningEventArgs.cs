namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerSpawningEventArgs : System.EventArgs
{
    public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
    public bool IsAllowed { get; set; } = true;

    public PlayerSpawningEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player) => Player = player;
}

public class PlayerSpawnedEventArgs : System.EventArgs
{
    public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
    public PlayerSpawnedEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player) => Player = player;
}