using System;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handlers;

public static class PlayerHandlers
{
    public static event Action<PlayerBannedEventArgs> Banned;
    public static event Action<PlayerChangedRoleEventArgs> ChangedRole;
    public static event Action<PlayerChangingRoleEventArgs> ChangingRole;
    public static event Action<PlayerDiedEventArgs> Died;
    public static event Action<PlayerDyingEventArgs> Dying;
    public static event Action<PlayerHurtingEventArgs> Hurting;
    public static event Action<PlayerJoinedEventArgs> Joined;
    public static event Action<PlayerKickedEventArgs>  Kicked;
    public static event Action<PlayerLeftEventArgs> Left;
    public static event Action<PlayerSpawningEventArgs> Spawning;
    public static event Action<PlayerSpawnedEventArgs> Spawned;
    public static event Action<PlayerVerifiedEventArgs> Verified;
    public static event Action<RespawnedTeamEventArgs> TeamRespawned;
    
    public static void InvokeSafely(PlayerBannedEventArgs ev) => Banned?.Invoke(ev);
    public static void InvokeSafely(PlayerChangedRoleEventArgs ev) => ChangedRole?.Invoke(ev);
    public static void InvokeSafely(PlayerChangingRoleEventArgs ev) => ChangingRole?.Invoke(ev);
    public static void InvokeSafely(PlayerDiedEventArgs ev) => Died?.Invoke(ev);
    public static void InvokeSafely(PlayerDyingEventArgs ev) => Dying?.Invoke(ev);
    public static void InvokeSafely(PlayerHurtingEventArgs ev) => Hurting?.Invoke(ev);
    public static void InvokeSafely(PlayerJoinedEventArgs ev) => Joined?.Invoke(ev);
    public static void InvokeSafely(PlayerKickedEventArgs ev) => Kicked?.Invoke(ev);
    public static void InvokeSafely(PlayerLeftEventArgs ev) => Left?.Invoke(ev);
    public static void InvokeSafely(PlayerSpawningEventArgs ev) => Spawning?.Invoke(ev);
    public static void InvokeSafely(PlayerSpawnedEventArgs ev) => Spawned?.Invoke(ev);
    public static void InvokeSafely(PlayerVerifiedEventArgs ev) => Verified?.Invoke(ev);
    public static void InvokeSafely(RespawnedTeamEventArgs ev) => TeamRespawned?.Invoke(ev);
}