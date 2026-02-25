using Interactables.Interobjects.DoorUtils;
using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerInteractingDoorEventArgs : IEventArgs
{
    /// <summary>
    /// Gets the player who is interacting with the door.
    /// </summary>
    public global::PurgaLib.API.Features.Player Player { get; }

    /// <summary>
    /// Gets the door being interacted with.
    /// </summary>
    public DoorVariant Door { get; }

    /// <summary>
    /// Gets the collider ID sent by the client.
    /// </summary>
    public byte ColliderId { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the player is allowed to open/close the door.
    /// </summary>
    public bool IsAllowed { get; set; } = true;

    public PlayerInteractingDoorEventArgs(
        global::PurgaLib.API.Features.Player player,
        DoorVariant door,
        byte colliderId)
    {
        Player = player;
        Door = door;
        ColliderId = colliderId;
    }
}