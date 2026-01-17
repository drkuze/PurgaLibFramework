using System;
using HarmonyLib;
using PlayerRoles;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler;

namespace PurgaLibEvents.PurgaLibEvent.Patch.Player;

[PurgaLibEventPatcher(typeof(PlayerHandler), nameof(PlayerHandler.OnChangingRole))]
[PurgaLibEventPatcher(typeof(PlayerHandler), nameof(PlayerHandler.OnChangedRole))]
[PurgaLibEventPatcher(typeof(PlayerHandler), nameof(PlayerHandler.OnSpawned))]
[PurgaLibEventPatcher(typeof(PlayerHandler), nameof(PlayerHandler.OnSpawning))]
[HarmonyPatch(typeof(PlayerRoleManager), nameof(PlayerRoleManager.InitializeNewRole))]
public static class ChangingRoleSpawnPatch
{
    private static void Prefix(PlayerRoleManager __instance, PlayerRoleBase oldRole ,RoleTypeId newRole)
    {
        try
        {
            var hub = __instance.Hub;
            if (hub == null) return;

            var player = new PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player(hub);
            
            PlayerHandler.OnSpawning(new PlayerSpawningEventArgs(player));
            PlayerHandler.OnChangingRole(new PlayerChangingRoleEventArgs(player, oldRole, newRole));
        }
        catch (Exception e)
        {
            Log.Error($"Error in ChangingRoleChangedRolePatch Prefix: {e}");
        }
    }

    private static void Postfix(PlayerRoleManager __instance, RoleTypeId oldRole ,PlayerRoleBase newRole)
    {
        try
        {
            var hub = __instance.Hub;
            if (hub == null) return;

            var player = new PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player(hub);
            
            PlayerHandler.OnSpawned(new PlayerSpawnedEventArgs(player));
            PlayerHandler.OnChangedRole(new PlayerChangedRoleEventArgs(player, oldRole, newRole, RoleChangeReason.None, RoleSpawnFlags.None));
        }
        catch (Exception e)
        {
            Log.Error($"Error in ChangingRoleChangedRolePatch Postfix: {e}");
        }
    }
}
