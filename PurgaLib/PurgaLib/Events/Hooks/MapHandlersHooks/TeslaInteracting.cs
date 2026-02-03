namespace PurgaLib.Events.Hooks.MapHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using Mirror;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Map;
    using PurgaLib.Events.Handlers;
    using System;

    [HarmonyPatch(typeof(TeslaGateController), nameof(TeslaGateController.ServerReceiveMessage))]
    public static class TeslaInteracting
    {
        public static void Prefix(NetworkConnection conn, TeslaHitMsg msg)
        {
            ReferenceHub hub;
            if (!ReferenceHub.TryGetHubNetID(conn.identity.netId, out hub)) return;
            if (hub == null || msg.Gate == null) return;

            var player = Player.Get(hub);
            if (player == null) return;

            var ev = new OnInteractingTeslaEventArgs(player, msg.Gate);
            try
            {
                MapHandlers.InvokeSafely(ev);
                if (!ev.IsAllowed)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] TeslaInteracting error:\n{ex}");
            }
        }
    }
}