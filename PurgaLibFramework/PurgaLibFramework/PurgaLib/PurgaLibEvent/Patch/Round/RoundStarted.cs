using GameCore;
using HarmonyLib;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Patch.Round;

[PurgaLibEventPatcher(typeof(RoundHandler), nameof(RoundHandler.OnStarted))]
[PurgaLibEventPatcher(typeof(RoundHandler), nameof(RoundHandler.OnStarting))]
[HarmonyPatch(typeof(RoundStart), nameof(RoundStart.RoundStarted))]
public static class RoundStarted
{
    private static void Prefix(bool value)
    {
        if (!value || value == RoundStart.RoundStarted)
            return;
        RoundHandler.OnStarting(new RoundStartingEventArgs());
        RoundHandler.OnStarted(new RoundStartedEventArgs());
    }
}