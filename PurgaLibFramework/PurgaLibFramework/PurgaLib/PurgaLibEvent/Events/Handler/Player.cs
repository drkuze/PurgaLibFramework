using PurgaLibEvents.PurgaLibEvent.Attribute;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomItems.EventsArgs;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class PlayerHandler
    {
        public static Event<PlayerJoinedEventArgs> Joined; 
        public static Event<PlayerLeftEventArgs> Left; 
        public static Event<PlayerVerifiedEventArgs> Verified; 
        public static Event<PlayerHurtingEventArgs> Hurting; 
        public static Event<PlayerDyingEventArgs> Dying;
        public static Event<PlayerDiedEventArgs> Died;
        public static Event<PlayerChangingRoleEventArgs> ChangingRole;
        public static Event<PlayerChangedRoleEventArgs> ChangedRole;
        public static Event<PlayerSpawningEventArgs> Spawning; 
        public static Event<PlayerSpawnedEventArgs> Spawned;
        public static Event<PlayerBannedEventArgs> Banned;
        public static Event<PlayerKickedEventArgs> Kicked;
        public static Event<UpgradingPlayersEventArgs> Upgrading;
        public static Event<CustomItemPickedUpEventArgs> ItemPickedUp;

        internal static void OnJoined(PlayerJoinedEventArgs ev) => Joined?.Invoke(ev);
        internal static void OnLeft(PlayerLeftEventArgs ev) => Left?.Invoke(ev);
        internal static void OnVerified(PlayerVerifiedEventArgs ev) => Verified?.Invoke(ev);
        internal static void OnHurting(PlayerHurtingEventArgs ev) => Hurting?.Invoke(ev);
        internal static void OnDying(PlayerDyingEventArgs ev) => Dying?.Invoke(ev);
        internal static void OnDied(PlayerDiedEventArgs ev) => Died?.Invoke(ev);
        internal static void OnChangingRole(PlayerChangingRoleEventArgs ev) => ChangingRole?.Invoke(ev);
        internal static void OnChangedRole(PlayerChangedRoleEventArgs ev) => ChangedRole?.Invoke(ev);
        internal static void OnSpawning(PlayerSpawningEventArgs ev) => Spawning?.Invoke(ev);
        internal static void OnSpawned(PlayerSpawnedEventArgs ev) => Spawned?.Invoke(ev);
        internal static void OnBanned(PlayerBannedEventArgs ev) => Banned?.Invoke(ev);
        internal static void OnKicked(PlayerKickedEventArgs ev) => Kicked?.Invoke(ev);
        internal static void OnUpgrading(UpgradingPlayersEventArgs ev) => Upgrading?.Invoke(ev);
        internal static void OnItemPickedUp(CustomItemPickedUpEventArgs ev) => ItemPickedUp?.Invoke(ev);
    }
}
