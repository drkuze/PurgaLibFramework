using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerInteractingEmergencyButtonEventArgs : IEventArgs
{
    /// <summary>
    /// Gets the emergency door release button being interacted with.
    /// </summary>
    public EmergencyDoorRelease EmergencyDoorRelease { get; }

    /// <summary>
    /// Gets the door controlled by the emergency button.
    /// </summary>
    public DoorVariant Door { get; }

    /// <summary>
    /// Gets the player who is interacting with the emergency button.
    /// </summary>
    public global::PurgaLib.API.Features.Player Player { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the interaction is allowed.
    /// </summary>
    public bool IsAllowed { get; set; } = true;

    public PlayerInteractingEmergencyButtonEventArgs(
        EmergencyDoorRelease emergencyDoorRelease,
        DoorVariant door,
        global::PurgaLib.API.Features.Player player)
    {
        EmergencyDoorRelease = emergencyDoorRelease;
        Door = door;
        Player = player;
    }
}