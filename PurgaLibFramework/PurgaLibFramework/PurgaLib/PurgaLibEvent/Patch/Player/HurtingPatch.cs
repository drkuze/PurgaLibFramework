using System;
using HarmonyLib;
using PlayerStatsSystem;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler;

namespace PurgaLibEvents.PurgaLibEvent.Patch.Player;

[PurgaLibEventPatcher(typeof(PlayerHandler), nameof(PlayerHandler.OnHurting))] 
[HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.DealDamage))]
public static class HurtingPatch
{
    private static void Postfix(PlayerStats __instance, ref float damage, PlayerStats target)
    {
        try
        {
            // __instance = attacker
            // target = victim

            var attackerHub = __instance.gameObject.GetComponent<ReferenceHub>();
            var victimHub = target?.gameObject.GetComponent<ReferenceHub>();

            if (attackerHub == null || victimHub == null)
                return;

            var attacker = new PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player(attackerHub);
            var victim = new PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player(victimHub);

            // Trigger PurgaLib OnHurting event
            PlayerHandler.OnHurting(new PlayerHurtingEventArgs(attacker, victim, damage));
        }
        catch (Exception e)
        {
            Log.Error($"Error in HurtingPatch Postfix: {e}");
        }
    }
}
