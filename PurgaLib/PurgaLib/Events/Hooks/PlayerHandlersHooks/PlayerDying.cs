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

    [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.DealDamage))]
    internal static class PlayerDying
    {
        internal static bool OnPlayerDying(PlayerStats stats, DamageHandlerBase handler)
        {
            StandardDamageHandler standardHandler = handler as StandardDamageHandler;
            if (standardHandler != null && stats.GetModule<HealthStat>().CurValue - standardHandler.Damage > 0)
                return true;

            Player player = Player.Get(stats._hub);

            if (player == null)
                return true;

            if (player.IsHost)
                return true;

            Player attacker = null;
            string damageTypeName = DamageType.None.ToString();

            if (handler is AttackerDamageHandler attackerHandler)
            {
                attacker = Player.Get(attackerHandler.Attacker.Hub);
            }

            int damageTypeInt = GetDamageType(handler);
            damageTypeName = ((DamageType)damageTypeInt).ToString();

            try
            {
                string attackerInfo = attacker != null ? $"{attacker.Nickname} ({attacker.UserId})" : "Environment/Suicide";

                var ev = new PlayerDyingEventArgs(player, attacker, damageTypeName);
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
                Logged.Error($"[PurgaLib] PlayerDying event error for PlayerID {stats._hub.PlayerId}:\n{ex}");
                return true;
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

        private static bool Prefix(PlayerStats __instance, DamageHandlerBase handler)
        {
            return OnPlayerDying(__instance, handler);
        }
    }
}
