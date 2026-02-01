namespace PurgaLib.Events.Hooks.RoundHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Round;
    using PurgaLib.Events.Handlers;
    using System;

    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.ForceRoundStart))]
    internal static class OnRoundStarting
    {
        private static void Prefix()
        {
            try
            {
                var ev = new RoundStartingEventArgs();
                RoundHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] RoundStarting event error:\n{ex}");
            }
        }
    }
}