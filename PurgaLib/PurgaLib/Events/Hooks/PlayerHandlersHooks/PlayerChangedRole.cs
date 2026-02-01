namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using PlayerRoles;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.Handlers;
    using System;

    [HarmonyPatch(typeof(PlayerRoleManager), nameof(PlayerRoleManager.ServerSetRole))]
    internal static class PlayerChangedRole
    {
        private static RoleTypeId storedOldRole;

        internal static void OnPlayerChangedRole(PlayerRoleManager roleManager, RoleTypeId newRole, RoleChangeReason reason, RoleSpawnFlags spawnFlags)
        {
            Player player = Player.Get(roleManager._hub);

            if (player == null)
            {
                Logged.Warn($"[PurgaLib] Player not found for ReferenceHub during role changed event");
                return;
            }

            if (player.IsHost)
                return;

            RoleTypeId oldRole = storedOldRole;

            if (oldRole == newRole)
                return;

            try
            {
                var ev = new PlayerChangedRoleEventArgs(player, oldRole, newRole);
                PlayerHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] PlayerChangedRole event error for PlayerID {roleManager._hub.PlayerId}:\n{ex}");
            }
        }

        private static void Prefix(PlayerRoleManager __instance)
        {
            storedOldRole = __instance.CurrentRole.RoleTypeId;
        }

        private static void Postfix(PlayerRoleManager __instance, RoleTypeId newRole, RoleChangeReason reason, RoleSpawnFlags spawnFlags)
        {
            OnPlayerChangedRole(__instance, newRole, reason, spawnFlags);
        }
    }
}
