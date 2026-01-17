using System;
using HarmonyLib;
using PlayerStatsSystem;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Enums;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler;

namespace PurgaLibEvents.PurgaLibEvent.Patch.Player;

[PurgaLibEventPatcher(typeof(PlayerHandler), nameof(PlayerHandler.OnDying))]
[PurgaLibEventPatcher(typeof(PlayerHandler), nameof(PlayerHandler.OnDied))]
[HarmonyPatch(typeof(PlayerStats), "KillPlayer", MethodType.Normal)]
public static class DyingDiedPatch
{
    private static void Prefix(PlayerStats __instance, PlayerStats source = null, string reason = "")
    {
        try
        {
            var hub = __instance.gameObject.GetComponent<ReferenceHub>();
            if (hub == null) return;

            var player = new PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player(hub);
            if (source != null)
            {
                var attackerHub = source.gameObject.GetComponent<ReferenceHub>();
                if (attackerHub != null)
                {
                    var attacker = new PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player(attackerHub);
                    PlayerHandler.OnDying(new PlayerDyingEventArgs(player, attacker, reason));
                }
            }
            
        }
        catch (Exception e)
        {
            Log.Error($"Error in DyingDiedPatch Prefix: {e}");
        }
    }

    private static void Postfix(PlayerStats __instance, PlayerStats source = null)
    {
        try
        {
            var hub = __instance.gameObject.GetComponent<ReferenceHub>();
            if (hub == null) return;
    
            var player = new PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player(hub);
            var attackerHub = source.gameObject.GetComponent<ReferenceHub>();
            var attacker = new PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player(attackerHub);
            
            PlayerHandler.OnDied(new PlayerDiedEventArgs(player, attacker, DamageType.None.GetHashCode()));
        }
        catch (Exception e)
        {
            Log.Error($"Error in DyingDiedPatch Postfix: {e}");
        }
    }
}
