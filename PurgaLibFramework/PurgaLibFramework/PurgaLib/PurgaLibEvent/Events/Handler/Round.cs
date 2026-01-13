using System;
using LabApi.Events.Handlers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class RoundHandler
    {
        private static bool _initialized;

        public static event Action<RoundStartingEventArgs> Starting;
        public static event Action<RoundStartedEventArgs> Started;
        public static event Action<RoundEndedEventArgs> Ended;
        public static event Action<RoundRestartingEventArgs> Restarting;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            ServerEvents.RoundStarting -= OnRoundStarting;
            ServerEvents.RoundEnded -= OnRoundEnded;

            ServerEvents.RoundStarting += OnRoundStarting;
            ServerEvents.RoundEnded += OnRoundEnded;
        }

        private static void OnRoundStarting(LabApi.Events.Arguments.ServerEvents.RoundStartingEventArgs ev)
        {
            Starting?.Invoke(new RoundStartingEventArgs());
            Started?.Invoke(new RoundStartedEventArgs());
        }

        private static void OnRoundEnded(LabApi.Events.Arguments.ServerEvents.RoundEndedEventArgs ev)
        {
            Ended?.Invoke(new RoundEndedEventArgs(ev.LeadingTeam.ToString()));
            Restarting?.Invoke(new RoundRestartingEventArgs());
        }

        public static void OnStarting(RoundStartingEventArgs ev) => Starting?.Invoke(ev);
        public static void OnStarted(RoundStartedEventArgs ev) => Started?.Invoke(ev);
        public static void OnEnded(RoundEndedEventArgs ev)
        {
            Ended?.Invoke(ev);
            Restarting?.Invoke(new RoundRestartingEventArgs());
        }
        public static void OnRestarting(RoundRestartingEventArgs ev) => Restarting?.Invoke(ev);
    }
}
