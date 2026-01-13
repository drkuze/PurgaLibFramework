using System;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCredit;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;
using PlayerBannedEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerBannedEventArgs;
using PlayerChangedRoleEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerChangedRoleEventArgs;
using PlayerChangingRoleEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerChangingRoleEventArgs;
using PlayerDyingEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerDyingEventArgs;
using PlayerHurtingEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerHurtingEventArgs;
using PlayerJoinedEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerJoinedEventArgs;
using PlayerKickedEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerKickedEventArgs;
using PlayerLeftEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerLeftEventArgs;
using PlayerSpawnedEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerSpawnedEventArgs;
using PlayerSpawningEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerSpawningEventArgs;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class PlayerHandler
    {
        private static bool _initialized;

        public static event Action<PlayerJoinedEventArgs> Joined;
        public static event Action<PlayerBannedEventArgs> Banned;
        public static event Action<PlayerKickedEventArgs> Kicked;
        public static event Action<PlayerLeftEventArgs> Left;
        public static event Action<PlayerHurtingEventArgs> Hurting;
        public static event Action<PlayerDyingEventArgs> Dying;
        public static event Action<PlayerChangingRoleEventArgs> ChangingRole;
        public static event Action<PlayerChangedRoleEventArgs> ChangedRole;
        public static event Action<PlayerSpawningEventArgs> Spawning;
        public static event Action<PlayerSpawnedEventArgs> Spawned;
        public static event Action<PlayerPickingUpItemEventArgs> PickingUpItem;
        public static event Action<PlayerDroppingItemEventArgs> DroppingItem;
        public static event Action<PlayerUsingItemEventArgs> UsingItem;
        public static event Action<PlayerInteractingDoorEventArgs> InteractingDoor;
        public static event Action<PlayerInteractingElevatorEventArgs> InteractingElevator;
        public static event Action<PlayerVerifiedEventArgs> Verified;
        public static event Action<UpgradingPlayersEventArgs> Upgrading;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            PlayerEvents.Joined += ev =>
            {
                var playerWrapper = Player.Get(ev.Player);
                Joined?.Invoke(new PlayerJoinedEventArgs(playerWrapper));

                string userId = ev.Player.UserId;
                if (string.IsNullOrEmpty(userId)) return;

                if (VerifiedPlayersCache.Verified.Add(userId))
                {
                    Verified?.Invoke(new PlayerVerifiedEventArgs(playerWrapper));
                    MEC.Timing.CallDelayed(0f, () =>
                    {
                        playerWrapper.SetRole(PlayerRoles.RoleTypeId.Spectator);
                        CreditVerifiedHandler.Handle(playerWrapper);
                    });
                }
            };

            PlayerEvents.Left += ev => Left?.Invoke(new PlayerLeftEventArgs(Player.Get(ev.Player)));
            PlayerEvents.Spawning += ev => Spawning?.Invoke(new PlayerSpawningEventArgs(Player.Get(ev.Player)));
            PlayerEvents.Spawned += ev => Spawned?.Invoke(new PlayerSpawnedEventArgs(Player.Get(ev.Player)));
            PlayerEvents.Dying += ev =>
            {
                var attacker = Player.Get(ev.Attacker);
                Dying?.Invoke(new PlayerDyingEventArgs(Player.Get(ev.Player), attacker, ev.DamageHandler.ToString()));
            };
            PlayerEvents.Hurting += ev =>
            {
                var attacker = Player.Get(ev.Attacker);
                Hurting?.Invoke(new PlayerHurtingEventArgs(Player.Get(ev.Player), attacker, ev.DamageHandler.GetHashCode()));
            };
            PlayerEvents.ChangingRole += ev =>
                ChangingRole?.Invoke(new PlayerChangingRoleEventArgs(Player.Get(ev.Player), ev.OldRole, ev.NewRole));
            PlayerEvents.ChangedRole += ev =>
                ChangedRole?.Invoke(new PlayerChangedRoleEventArgs(
                    Player.Get(ev.Player),
                    ev.OldRole,
                    ev.NewRole,
                    ev.ChangeReason,
                    ev.SpawnFlags));
            PlayerEvents.Banned += ev =>
                Banned?.Invoke(new PlayerBannedEventArgs(Player.Get(ev.Player), ev.Reason, ev.Duration));
            PlayerEvents.Kicked += ev =>
                Kicked?.Invoke(new PlayerKickedEventArgs(Player.Get(ev.Player), ev.Reason));
            Scp914Events.ProcessedPlayer += ev =>
                Upgrading?.Invoke(new UpgradingPlayersEventArgs(Player.Get(ev.Player), ev.KnobSetting));
            PlayerEvents.PickingUpItem += ev =>
                PickingUpItem?.Invoke(new PlayerPickingUpItemEventArgs(ev.Player.ReferenceHub, ev.Pickup.Base));
            PlayerEvents.DroppingItem += ev =>
                DroppingItem?.Invoke(new PlayerDroppingItemEventArgs(ev.Player.ReferenceHub, ev.Item.Base, ev.Throw));
            PlayerEvents.UsingItem += ev =>
                UsingItem?.Invoke(new PlayerUsingItemEventArgs(ev.Player.ReferenceHub, ev.UsableItem.Base));
            PlayerEvents.InteractingDoor += ev =>
                InteractingDoor?.Invoke(new PlayerInteractingDoorEventArgs(ev.Player.ReferenceHub, ev.Door.Base, ev.IsAllowed));
            PlayerEvents.InteractingElevator += ev =>
                InteractingElevator?.Invoke(new PlayerInteractingElevatorEventArgs(ev.Player.ReferenceHub, ev.Elevator.Base, ev.Panel));
        }
    }
}
