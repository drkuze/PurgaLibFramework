namespace PurgaLib.Events.Hooks.RoundHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Round;
    using PurgaLib.Events.Handlers;
    using System;

    [HarmonyPatch(typeof(RoundSummary), nameof(RoundSummary.RpcShowRoundSummary))]
    internal static class OnRoundEnded
    {
        private static void Prefix(RoundSummary.LeadingTeam leadingTeam)
        {
            try
            {
                var ev = new RoundEndedEventArgs(leadingTeam);
                RoundHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] RoundEnded event error:\n{ex}");
            }
        }
    }
}