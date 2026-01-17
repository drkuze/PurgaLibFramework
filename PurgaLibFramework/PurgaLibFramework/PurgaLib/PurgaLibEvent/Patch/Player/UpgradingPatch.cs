using System;
using HarmonyLib;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler;
using Scp914;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Patch.Player;

[PurgaLibEventPatcher(typeof(PlayerHandler), nameof(PlayerHandler.Upgrading))]
[HarmonyPatch(typeof(Scp914Upgrader), "ProcessPlayer", MethodType.Normal)]
public static class UpgradingPatch
{
    private static void Prefix(ReferenceHub playerHub, Scp914KnobSetting knobSetting)
    {
        try
        {
            if (playerHub == null) return;

            var player = new PurgaLibAPI.Features.Player(playerHub);
            
            PlayerHandler.OnUpgrading(new UpgradingPlayersEventArgs(player, knobSetting));
        }
        catch (Exception e)
        {
            Log.Error($"Error in UpgradingPatch Prefix: {e}");
        }
    }
}
