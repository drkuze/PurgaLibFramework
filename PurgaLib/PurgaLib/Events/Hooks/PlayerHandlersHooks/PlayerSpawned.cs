namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using PlayerRoles;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;
    using System;

    [HarmonyPatch(typeof(PlayerRoleManager), nameof(PlayerRoleManager.InitializeNewRole))]
    internal static class PlayerSpawned
    {
        internal static void OnPlayerSpawned(PlayerRoleManager roleManager, RoleTypeId targetId)
        {
            Player player = Player.Get(roleManager._hub);

            if (player == null)
            {
                Logged.Warn($"[PurgaLib] Player not found for ReferenceHub during spawned event");
                return;
            }

            if (player.IsHost)
                return;

            if (targetId == RoleTypeId.Spectator)
                return;

            try
            {
                PlayerHandlers.InvokeSafely(new PlayerSpawnedEventArgs(player));
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] PlayerSpawned event error for PlayerID {roleManager._hub.PlayerId}:\n{ex}");
            }
        }

        private static void Postfix(PlayerRoleManager __instance, RoleTypeId targetId, RoleChangeReason reason)
        {
            OnPlayerSpawned(__instance, targetId);
        }
    }
}
