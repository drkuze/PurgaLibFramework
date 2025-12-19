using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round;
using Player = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler.Player;
using PlayerChangingRoleEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerChangingRoleEventArgs;
using PlayerDyingEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerDyingEventArgs;
using PlayerJoinedEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerJoinedEventArgs;
using PlayerLeftEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerLeftEventArgs;
using PlayerSpawnedEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerSpawnedEventArgs;
using PlayerSpawningEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player.PlayerSpawningEventArgs;
using Round = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler.Round;
using RoundEndedEventArgs = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round.RoundEndedEventArgs;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Hooks
{
    public static class LabHook
    {
        public static void Register()
        {
            PlayerEvents.Joined += ev => Player.OnJoined(new PlayerJoinedEventArgs(ev.Player));
            PlayerEvents.Left += ev => Player.OnLeft(new PlayerLeftEventArgs(ev.Player));
            PlayerEvents.Spawning += ev => Player.OnSpawning(new PlayerSpawningEventArgs(ev.Player));
            PlayerEvents.Spawned += ev => Player.OnSpawned(new PlayerSpawnedEventArgs(ev.Player));
            PlayerEvents.Dying += ev =>
            {
                var args = new PlayerDyingEventArgs(ev.Player, ev.Attacker, ev.DamageHandler.ToString());
                Player.OnDying(args);
                if (!args.IsAllowed) ev.IsAllowed = false;
            };
            PlayerEvents.ChangingRole += ev =>
            {
                var args = new PlayerChangingRoleEventArgs(ev.Player, ev.NewRole, ev.OldRole);
                Player.OnChangingRole(args);
                if (!args.IsAllowed) ev.IsAllowed = false;
            };
            PlayerEvents.ChangedRole += ev => Player.OnChangedRole(new PlayerChangedRoleEventArgs(
                ev.Player.ReferenceHub,           
                ev.OldRole,            
                ev.NewRole,            
                ev.ChangeReason,       
                ev.SpawnFlags                     
            ));
            
            Round.WaitingForPlayers += (_, __) => Round.OnWaitingForPlayers(new WaitingForPlayersEventArgs());
            Round.Started += (_, __) => Round.OnStarted(new RoundStartedEventArgs());
            Round.Ended += (sender, ev) => Round.OnEnded(new RoundEndedEventArgs(ev.LeadingTeam.ToString()));
            Round.Restarting += (_, __) => Round.OnRestarting(new RoundRestartingEventArgs());
            
        }
    }
}
