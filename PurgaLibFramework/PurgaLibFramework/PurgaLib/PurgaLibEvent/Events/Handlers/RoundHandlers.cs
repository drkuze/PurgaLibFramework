using System;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handlers;

public static class RoundHandlers
{
    public static event Action<RoundEndedEventArgs> Ended;
    public static event Action<RoundStartedEventArgs> Started;
    public static event Action<RoundStartingEventArgs> Starting;
    public static event Action<RoundRestartingEventArgs> Restarting;
    
    public static void InvokeSafely(RoundEndedEventArgs ev) => Ended?.Invoke(ev);
    public static void InvokeSafely(RoundStartedEventArgs ev) => Started?.Invoke(ev);
    public static void InvokeSafely(RoundStartingEventArgs ev) => Starting?.Invoke(ev);
    public static void InvokeSafely(RoundRestartingEventArgs ev) => Restarting?.Invoke(ev);
}