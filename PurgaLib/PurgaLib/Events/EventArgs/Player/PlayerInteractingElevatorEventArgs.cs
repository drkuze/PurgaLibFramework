using Interactables.Interobjects;
using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerInteractingElevatorEventArgs : IEventArgs
{
    /// <summary>
    /// Gets the player who is interacting with the elevator.
    /// </summary>
    public global::PurgaLib.API.Features.Player Player { get; }

    /// <summary>
    /// Gets the elevator chamber being interacted with.
    /// </summary>
    public ElevatorChamber Chamber { get; }

    /// <summary>
    /// Gets a value indicating whether the player is interacting from inside the elevator.
    /// </summary>
    public bool IsInside { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the interaction is allowed.
    /// </summary>
    public bool IsAllowed { get; set; } = true;

    public PlayerInteractingElevatorEventArgs(
        global::PurgaLib.API.Features.Player player,
        ElevatorChamber chamber,
        bool isInside)
    {
        Player = player;
        Chamber = chamber;
        IsInside = isInside;
    }
}