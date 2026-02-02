namespace PurgaLib.Events.Hooks.RoundHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Round;
    using PurgaLib.Events.Handlers;
    using RoundRestarting;
    using System;

    [HarmonyPatch(typeof(RoundRestart), nameof(RoundRestart.InitiateRoundRestart))]
    internal static class OnRoundRestarting
    {
        private static bool Prefix()
        {
            try
            {
                var ev = new RoundRestartingEventArgs("Round restart initiated");
                RoundHandlers.InvokeSafely(ev);

                if (ev.IsAllowed) { return true; } else { return false; }
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] RoundRestarting event error:\n{ex}");
                return true;
            }
        }
    }
}