namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using Mirror;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;
    using System;

    [HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.OnDestroy))]
    internal static class PlayerLeft
    {
        internal static void OnPlayerLeft(ReferenceHub hub)
        {
            if (!NetworkServer.active)
                return;

            Player player = Player.Get(hub);

            if (player == null)
                return;

            if (player.IsHost)
                return;

            try
            {
                PlayerHandlers.InvokeSafely(new PlayerLeftEventArgs(player));
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] PlayerLeft event error for PlayerID {hub.PlayerId}:\n{ex}");
            }
        }

        private static void Prefix(ReferenceHub __instance)
        {
            OnPlayerLeft(__instance);
        }
    }
}