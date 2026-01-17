using System;
using HarmonyLib;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler;

namespace PurgaLibEvents.PurgaLibEvent.Patch.Player;

[PurgaLibEventPatcher(typeof(PlayerHandler), nameof(PlayerHandler.OnBanned))]
[HarmonyPatch(typeof(BanHandler), nameof(BanHandler.IssueBan))]
public static class BannedPatch
{
    private static void Postfix(string userId, string reason, long duration)
    {
        try
        {
            if (string.IsNullOrEmpty(userId)) return;
            
            var player = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player.Get(userId);
            
            PlayerHandler.OnBanned(new PlayerBannedEventArgs(player, reason, duration));
        }
        catch (Exception e)
        {
            Log.Error($"Error in BannedPatch Postfix: {e}");
        }
    }
}
