namespace PurgaLib.Events.Hooks.ServerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Server;
    using PurgaLib.Events.Handlers;
    using System;

    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.ForceRoundStart))]
    internal static class OnRoundStarted
    {
        private static void Postfix()
        {
            try
            {
                var ev = new RoundStartedEventArgs();
                ServerHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] RoundStarted event error:\n{ex}");
            }
        }
    }
}

