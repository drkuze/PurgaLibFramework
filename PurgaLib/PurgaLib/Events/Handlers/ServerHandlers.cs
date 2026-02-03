using System;
using PurgaLib.Events.EventArgs.Server;

namespace PurgaLib.Events.Handlers;

public static class ServerHandlers
{
    public static event Action<RoundEndedEventArgs> Ended;
    public static event Action<RoundStartedEventArgs> Started;
    public static event Action<RoundStartingEventArgs> Starting;
    public static event Action<RoundRestartingEventArgs> Restarting;
    public static event Action<WaitingForPlayersEventArgs> WaitingForPlayers;

    public static void InvokeSafely(RoundEndedEventArgs ev) => Ended?.Invoke(ev);
    public static void InvokeSafely(RoundStartedEventArgs ev) => Started?.Invoke(ev);
    public static void InvokeSafely(RoundStartingEventArgs ev) => Starting?.Invoke(ev);
    public static void InvokeSafely(RoundRestartingEventArgs ev) => Restarting?.Invoke(ev);
    public static void InvokeSafely(WaitingForPlayersEventArgs ev) => WaitingForPlayers?.Invoke(ev);
}