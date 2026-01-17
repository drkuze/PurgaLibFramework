using System;
using HarmonyLib;
using PlayerRoles;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCredit;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Patch.Player;

[PurgaLibEventPatcher(typeof(PlayerHandler), nameof(PlayerHandler.OnJoined))]
[HarmonyPatch(typeof(ReferenceHub), "Start", MethodType.Normal)]
public static class JoinedPatch
{
    private static void Postfix(ReferenceHub __instance)
    {
        try
        {
            var player = new global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player(__instance);
            var verp = new PlayerVerifiedEventArgs(player);
            var joined = new PlayerJoinedEventArgs(player);
            PlayerHandler.OnJoined(joined);
            string userId = player.UserId;
            if (string.IsNullOrEmpty(userId))
                return;
    
            if (VerifiedPlayersCache.Verified.Add(userId))
            {
                PlayerHandler.OnVerified(verp);

                MEC.Timing.CallDelayed(0f, () =>
                {
                    verp.Player.SetRole(RoleTypeId.Spectator);
                    CreditVerifiedHandler.Handle(player);
                });
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error in OnJoined: {e}");
        }
    }
}
