namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using PlayerStatsSystem;
    using PurgaLib.API.Enums;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;
    using System;

    [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.KillPlayer))]
    internal static class PlayerDied
    {
        internal static void OnPlayerDied(PlayerStats stats, DamageHandlerBase handler)
        {
            Player player = Player.Get(stats._hub);

            if (player == null)
            {
                Logged.Warn($"[PurgaLib] Player not found for ReferenceHub during death event");
                return;
            }

            if (player.IsHost)
                return;

            Player attacker = null;
            int damageType = (int)DamageType.None;

            if (handler is AttackerDamageHandler attackerHandler)
            {
                attacker = Player.Get(attackerHandler.Attacker.Hub);
            }

            damageType = GetDamageType(handler);

            try
            {
                PlayerHandlers.InvokeSafely(new PlayerDiedEventArgs(player, attacker, damageType));
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] PlayerDied event error for PlayerID {stats._hub.PlayerId}:\n{ex}");
            }
        }

        private static int GetDamageType(DamageHandlerBase handler)
        {
            if (handler == null)
                return (int)DamageType.None;

            string handlerTypeName = handler.GetType().Name;

            if (handler is FirearmDamageHandler)
                return (int)DamageType.Bullet;

            if (handler is ExplosionDamageHandler)
                return (int)DamageType.Explosion;

            if (handlerTypeName.Contains("Fall"))
                return (int)DamageType.Fall;

            if (handlerTypeName.Contains("Fire") || handlerTypeName.Contains("Burn"))
                return (int)DamageType.Fire;

            if (handler is JailbirdDamageHandler || handlerTypeName.Contains("Melee"))
                return (int)DamageType.Melee;

            if (handlerTypeName.Contains("Flash"))
                return (int)DamageType.Flash;

            if (handlerTypeName.Contains("Decontamination") || handlerTypeName.Contains("Decon"))
                return (int)DamageType.Decontamination;

            if (handlerTypeName.Contains("Warhead"))
                return (int)DamageType.Warhead;

            if (handlerTypeName.Contains("Scp207") || handlerTypeName.Contains("207"))
                return (int)DamageType.SCP207;

            if (handlerTypeName.Contains("Scp3114") || handlerTypeName.Contains("3114"))
                return (int)DamageType.SCP372;

            return (int)DamageType.None;
        }

        private static void Prefix(PlayerStats __instance, DamageHandlerBase handler)
        {
            OnPlayerDied(__instance, handler);
        }
    }
}
