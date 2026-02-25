using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerChangingSpectatedEventArgs : IEventArgs
{
    /// <summary>
    /// Gets the spectator (the player whose spectated target is changing).
    /// </summary>
    public global::PurgaLib.API.Features.Player Player { get; }

    /// <summary>
    /// Gets the previously spectated player. May be <see langword="null"/> if none.
    /// </summary>
    public global::PurgaLib.API.Features.Player OldTarget { get; }

    /// <summary>
    /// Gets or sets the new player to spectate. May be <see langword="null"/> if switching to freecam/none.
    /// </summary>
    public global::PurgaLib.API.Features.Player NewTarget { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the spectated player change is allowed.
    /// </summary>
    public bool IsAllowed { get; set; } = true;

    public PlayerChangingSpectatedEventArgs(
        global::PurgaLib.API.Features.Player player,
        global::PurgaLib.API.Features.Player oldTarget,
        global::PurgaLib.API.Features.Player newTarget)
    {
        Player = player;
        OldTarget = oldTarget;
        NewTarget = newTarget;
    }
}