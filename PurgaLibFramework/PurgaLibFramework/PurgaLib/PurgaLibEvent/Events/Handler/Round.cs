using System;
using LabApi.Events.Handlers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLib_API.Server;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class RoundHandler
    {
        private static EventHandler<RoundStartedEventArgs> _started;
        private static EventHandler<RoundEndedEventArgs> _ended;
        private static EventHandler<RoundRestartingEventArgs> _restarting;
        private static EventHandler<WaitingForPlayersEventArgs> _waitingForPlayers;
        private static EventHandler<RoundStartingEventArgs> _starting;

        public static event EventHandler<RoundStartedEventArgs> Started
        {
            add { Add(ref _started, value, RegisterLabApi); }
            remove { _started -= value; }
        }

        public static event EventHandler<RoundEndedEventArgs> Ended
        {
            add { Add(ref _ended, value, RegisterLabApi); }
            remove { _ended -= value; }
        }

        public static event EventHandler<RoundRestartingEventArgs> Restarting
        {
            add { Add(ref _restarting, value, RegisterLabApi); }
            remove { _restarting -= value; }
        }

        public static event EventHandler<WaitingForPlayersEventArgs> WaitingForPlayers
        {
            add { Add(ref _waitingForPlayers, value, RegisterLabApi); }
            remove { _waitingForPlayers -= value; }
        }

        public static event EventHandler<RoundStartingEventArgs> Starting
        {
            add { Add(ref _starting, value, RegisterLabApi); }
            remove { _starting -= value; }
        }

        private static void Add<T>(ref EventHandler<T> field, EventHandler<T> handler, Action register)
        {
            bool wasEmpty = field == null;
            field += handler;
            if (wasEmpty)
                register();
        }

        private static void On<T>(EventHandler<T> field, T args) where T : System.EventArgs
        {
            field?.Invoke(null, args);
        }

        public static void RegisterLabApi()
        {
            ServerEvents.RoundStarted += RoundStarted;
            ServerEvents.RoundEnded += RoundEnding;
            ServerEvents.RoundStarting += RoundStarting;
                            
            Log.Success("[PurgaLib] RoundHandler registered on LabApi.");
        }
        
        
        private static void RoundStarted()
        {
            On(_started, new RoundStartedEventArgs());
        }

        private static void RoundEnding(LabApi.Events.Arguments.ServerEvents.RoundEndedEventArgs ev)
        {
            On(_ended, new RoundEndedEventArgs(ev.LeadingTeam.ToString()));
            On(_restarting, new RoundRestartingEventArgs());
        }

        private static void RoundStarting(LabApi.Events.Arguments.ServerEvents.RoundStartingEventArgs ev)
        {
            On(_starting, new RoundStartingEventArgs());
        }
        
        public static void OnStarted(RoundStartedEventArgs ev) => _started?.Invoke(null, ev);
        public static void OnEnded(RoundEndedEventArgs ev) => _ended?.Invoke(null, ev);
        public static void OnRestarting(RoundRestartingEventArgs ev) => _restarting?.Invoke(null, ev);
        public static void OnWaitingForPlayers(WaitingForPlayersEventArgs ev) => _waitingForPlayers?.Invoke(null, ev);
        public static void OnStarting(RoundStartingEventArgs ev) => _starting?.Invoke(null, ev);
    }
}
