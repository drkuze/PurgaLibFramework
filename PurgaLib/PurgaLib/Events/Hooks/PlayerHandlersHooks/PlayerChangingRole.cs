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

    [HarmonyPatch(typeof(PlayerRoleManager), nameof(PlayerRoleManager.ServerSetRole))]
    internal static class PlayerChangingRole
    {

        internal static bool OnPlayerChangingRole(PlayerRoleManager roleManager, RoleTypeId newRole, RoleChangeReason reason, RoleSpawnFlags spawnFlags)
        {
            Player player = Player.Get(roleManager._hub);

            if (player == null)
                return true;

            if (player.IsHost)
                return true;

            RoleTypeId oldRole = roleManager.CurrentRole.RoleTypeId;

            if (oldRole == newRole)
                return true;

            try
            {
                var ev = new PlayerChangingRoleEventArgs(player, oldRole, newRole);
                PlayerHandlers.InvokeSafely(ev);

                if (ev.IsAllowed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] PlayerChangingRole event error for PlayerID {roleManager._hub.PlayerId}:\n{ex}");
                return true;
            }
        }

        private static bool Prefix(PlayerRoleManager __instance, RoleTypeId newRole, RoleChangeReason reason, RoleSpawnFlags spawnFlags)
        {
            return OnPlayerChangingRole(__instance, newRole, reason, spawnFlags);
        }
    }
}