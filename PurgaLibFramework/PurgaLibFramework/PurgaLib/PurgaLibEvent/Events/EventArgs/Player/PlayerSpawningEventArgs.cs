namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerSpawningEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Player { get; }
    public bool IsAllowed { get; set; } = true;

    public PlayerSpawningEventArgs(LabApi.Features.Wrappers.Player player) => Player = player;
}

public class PlayerSpawnedEventArgs : System.EventArgs
{
    public LabApi.Features.Wrappers.Player Player { get; }
    public PlayerSpawnedEventArgs(LabApi.Features.Wrappers.Player player) => Player = player;
}