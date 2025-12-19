using System;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class Round
    {
        public static event EventHandler<RoundStartedEventArgs> Started;
        public static event EventHandler<RoundEndedEventArgs> Ended;
        public static event EventHandler<RoundRestartingEventArgs> Restarting;
        public static event EventHandler<WaitingForPlayersEventArgs> WaitingForPlayers;

        internal static void OnStarted(RoundStartedEventArgs ev)
            => EventManager.Invoke(Started, null, ev);

        internal static void OnEnded(RoundEndedEventArgs ev)
            => EventManager.Invoke(Ended, null, ev);

        internal static void OnRestarting(RoundRestartingEventArgs ev)
            => EventManager.Invoke(Restarting, null, ev);
        
        internal static void OnWaitingForPlayers(WaitingForPlayersEventArgs ev)
            => EventManager.Invoke(WaitingForPlayers, null, ev);
    }
}