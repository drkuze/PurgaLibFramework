namespace PurgaLib.Events.Hooks.ServerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Server;
    using PurgaLib.Events.Handlers;
    using RoundRestarting;
    using System;

    [HarmonyPatch(typeof(RoundSummary), nameof(RoundSummary.Start))]
    internal static class OnWaitingForPlayers_ServerStart
    {
        private static void Postfix()
        {
            try
            {
                var ev = new WaitingForPlayersEventArgs();
                ServerHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] WaitingForPlayers event error:\n{ex}");
            }
        }
    }

    [HarmonyPatch(typeof(RoundRestart), nameof(RoundRestart.InitiateRoundRestart))]
    internal static class OnWaitingForPlayers_RoundRestart
    {
        private static void Postfix()
        {
            try
            {
                var ev = new WaitingForPlayersEventArgs();
                ServerHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] WaitingForPlayers event error:\n{ex}");
            }
        }
    }
}