namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using PlayerStatsSystem;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;
    using System;


    [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.DealDamage))]
    internal static class PlayerHurting
    {
        internal static bool OnPlayerHurting(PlayerStats stats, DamageHandlerBase handler)
        {
            StandardDamageHandler standardHandler = handler as StandardDamageHandler;
            if (standardHandler != null && stats.GetModule<HealthStat>().CurValue - standardHandler.Damage <= 0)
                return true;

            Player target = Player.Get(stats._hub);

            if (target == null)
            {
                Logged.Warn($"[PurgaLib] Target player not found for ReferenceHub during hurting event");
                return true;
            }

            if (target.IsHost)
                return true;

            Player attacker = null;
            float damage = 0f;

            if (handler is AttackerDamageHandler attackerHandler)
            {
                attacker = Player.Get(attackerHandler.Attacker.Hub);
            }

            if (standardHandler != null)
            {
                damage = standardHandler.Damage;
            }

            try
            {
                var ev = new PlayerHurtingEventArgs(attacker, target, damage);
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
                Logged.Error($"[PurgaLib] PlayerHurting event error for PlayerID {stats._hub.PlayerId}:\n{ex}");
                return true;
            }
        }

        private static bool Prefix(PlayerStats __instance, DamageHandlerBase handler)
        {
            return OnPlayerHurting(__instance, handler);
        }
    }
}