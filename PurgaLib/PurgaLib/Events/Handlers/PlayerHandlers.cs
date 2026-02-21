using PurgaLib.Events.EventArgs.Player;
using System;

namespace PurgaLib.Events.Handlers;

public static class PlayerHandlers
{
    public static event Action<PlayerBannedEventArgs> Banned;
    public static event Action<PlayerChangedRoleEventArgs> ChangedRole;
    public static event Action<PlayerChangingRoleEventArgs> ChangingRole;
    public static event Action<PlayerDiedEventArgs> Died;
    public static event Action<PlayerDyingEventArgs> Dying;
    public static event Action<PlayerHurtingEventArgs> Hurting;
    public static event Action<PlayerInteractingDoorEventArgs> InteractingDoor;
    public static event Action<PlayerInteractingElevatorEventArgs> InteractingElevator;
    public static event Action<PlayerInteractingEmergencyButtonEventArgs> InteractingEmergencyButton;
    public static event Action<PlayerJoinedEventArgs> Joined;
    public static event Action<PlayerKickedEventArgs>  Kicked;
    public static event Action<PlayerLeftEventArgs> Left;
    public static event Action<PlayerSpawningEventArgs> Spawning;
    public static event Action<PlayerSpawnedEventArgs> Spawned;
    public static event Action<PlayerDroppingItemEventArgs> DroppingItem;
    public static event Action<PlayerVoiceChattingEventArgs> VoiceChatting;

    public static void InvokeSafely(PlayerBannedEventArgs ev) => Banned?.Invoke(ev);
    public static void InvokeSafely(PlayerChangedRoleEventArgs ev) => ChangedRole?.Invoke(ev);
    public static void InvokeSafely(PlayerChangingRoleEventArgs ev) => ChangingRole?.Invoke(ev);
    public static void InvokeSafely(PlayerDiedEventArgs ev) => Died?.Invoke(ev);
    public static void InvokeSafely(PlayerDyingEventArgs ev) => Dying?.Invoke(ev);
    public static void InvokeSafely(PlayerHurtingEventArgs ev) => Hurting?.Invoke(ev);
    public static void InvokeSafely(PlayerInteractingDoorEventArgs ev) => InteractingDoor?.Invoke(ev);
    public static void InvokeSafely(PlayerInteractingElevatorEventArgs ev) => InteractingElevator?.Invoke(ev);
    public static void InvokeSafely(PlayerInteractingEmergencyButtonEventArgs ev) => InteractingEmergencyButton?.Invoke(ev);
    public static void InvokeSafely(PlayerJoinedEventArgs ev) => Joined?.Invoke(ev);
    public static void InvokeSafely(PlayerKickedEventArgs ev) => Kicked?.Invoke(ev);
    public static void InvokeSafely(PlayerLeftEventArgs ev) => Left?.Invoke(ev);
    public static void InvokeSafely(PlayerSpawningEventArgs ev) => Spawning?.Invoke(ev);
    public static void InvokeSafely(PlayerSpawnedEventArgs ev) => Spawned?.Invoke(ev);
    public static void InvokeSafely(PlayerDroppingItemEventArgs ev) => DroppingItem?.Invoke(ev);
    public static void InvokeSafely(PlayerVoiceChattingEventArgs ev) => VoiceChatting?.Invoke(ev);
}