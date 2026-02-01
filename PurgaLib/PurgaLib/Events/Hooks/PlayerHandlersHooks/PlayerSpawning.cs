namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using MEC;
    using PlayerRoles;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;
    using System;

    [HarmonyPatch(typeof(PlayerRoleManager), nameof(PlayerRoleManager.InitializeNewRole))]
    internal static class PlayerSpawning
    {
        internal static bool OnPlayerSpawning(PlayerRoleManager roleManager, RoleTypeId targetId, RoleChangeReason reason, RoleSpawnFlags spawnFlags)
        {
            if (targetId == RoleTypeId.Spectator)
                return true;


            Player player = Player.Get(roleManager._hub);

            if (player == null)
            {
                Timing.CallDelayed(0.1f, () =>
                {
                    Player delayedPlayer = Player.Get(roleManager._hub);
                    if (delayedPlayer != null && !delayedPlayer.IsHost)
                    {
                        try
                        {
                            var ev = new PlayerSpawningEventArgs(delayedPlayer);
                            PlayerHandlers.InvokeSafely(ev);
                        }
                        catch (Exception ex)
                        {
                            Logged.Error($"[PurgaLib] PlayerSpawning (delayed) event error for PlayerID {roleManager._hub.PlayerId}:\n{ex}");
                        }
                    }
                });

                return true;
            }

            try
            {
                var ev = new PlayerSpawningEventArgs(player);
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
                Logged.Error($"[PurgaLib] PlayerSpawning event error for PlayerID {roleManager._hub.PlayerId}:\n{ex}");
                return true;
            }
        }

        private static bool Prefix(PlayerRoleManager __instance, RoleTypeId targetId, RoleChangeReason reason, RoleSpawnFlags spawnFlags)
        {
            return OnPlayerSpawning(__instance, targetId, reason, spawnFlags);
        }
    }
}