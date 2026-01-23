using HarmonyLib;
using GameCore;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using RoundRestarting;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handlers;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Hooks
{
    [HarmonyPatch]
    public static class RoundHooks
    {
        [HarmonyPatch(typeof(RoundStart), nameof(RoundStart.NetworkTimer), MethodType.Setter)]
        [HarmonyPostfix]
        public static void OnNetworkTimerSet(short value)
        {
            if (value == -1)
            {
                var evStarting = new RoundStartingEventArgs();
                try { RoundHandlers.InvokeSafely(evStarting); }
                catch (System.Exception ex) { Log.Error($"[PurgaLib] RoundStarting error:\n{ex}"); }
                
                var evStarted = new RoundStartedEventArgs();
                try { RoundHandlers.InvokeSafely(evStarted); }
                catch (System.Exception ex) { Log.Error($"[PurgaLib] RoundStarted error:\n{ex}"); }
            }
        }
        
        public static void InitRoundEndHook()
        {
            RoundSummary.OnRoundEnded += (RoundSummary.LeadingTeam leadingTeam, RoundSummary.SumInfo_ClassList sumInfo) =>
            {
                var ev = new RoundEndedEventArgs(leadingTeam);
                try { RoundHandlers.InvokeSafely(ev); }
                catch (System.Exception ex) { Log.Error($"[PurgaLib] RoundEnded error:\n{ex}"); }
            };
        }
        
        public static void InitRoundRestartHook()
        {
            RoundRestart.OnRestartTriggered += () =>
            {
                var ev = new RoundRestartingEventArgs();
                try { RoundHandlers.InvokeSafely(ev); }
                catch (System.Exception ex) { Log.Error($"[PurgaLib] RoundRestarting error:\n{ex}"); }
            };
        }
    }
}
