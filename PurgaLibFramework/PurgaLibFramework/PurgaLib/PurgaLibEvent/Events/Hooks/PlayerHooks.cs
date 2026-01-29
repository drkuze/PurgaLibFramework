using System;
using CentralAuth;
using HarmonyLib;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Spawnpoints;
using PlayerStatsSystem;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Extensions;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handlers;
using Respawning;
using Respawning.Waves;
using UnityEngine;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Hooks
{
    [HarmonyPatch]
    public static class PlayerHooks
    {
        [HarmonyPatch(typeof(PlayerAuthenticationManager), nameof(PlayerAuthenticationManager.FinalizeAuthentication))]
        public static class Patch_PlayerAuthenticationManager_FinalizeAuthentication
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerAuthenticationManager __instance)
            {
                var hub = __instance._hub;
                if (hub == null) return;

                var player = PurgaLibAPI.Features.Player.Get(hub);
                if (player == null) return;

                try { PlayerHandlers.InvokeSafely(new PlayerVerifiedEventArgs(player)); }
                catch (Exception ex) { Log.Error($"[PurgaLib] Verified error:\n{ex}"); }
            }
        }
        
        [HarmonyPatch(typeof(ReferenceHub), nameof(ReferenceHub.Start))]
        [HarmonyPostfix]
        public static void Postfix_Joined(ReferenceHub __instance)
        {
            if (!NetworkServer.active) return;

            var player = PurgaLibAPI.Features.Player.Get(__instance);
            if (player == null)
            {
                Log.Warn($"[PurgaLib] Player not found for ReferenceHub {__instance.PlayerId}");
                return;
            }

            try { PlayerHandlers.InvokeSafely(new PlayerJoinedEventArgs(player)); }
            catch (Exception ex) { Log.Error($"[PurgaLib] Joined error for PlayerID {__instance.PlayerId}:\n{ex}"); }
        }

        [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.Disconnect), new Type[] { typeof(GameObject), typeof(string) })]
        [HarmonyPostfix]
        public static void Postfix_Disconnect(GameObject player, string message) 
        {
            if (player == null) return;

            if (!ReferenceHub.TryGetHub(player, out var hub)) return;
            var pl = PurgaLibAPI.Features.Player.Get(hub);
            if (pl == null) return;

            try
            {
                PlayerHandlers.InvokeSafely(new PlayerLeftEventArgs(pl));
                PlayerHandlers.InvokeSafely(new PlayerKickedEventArgs(pl, message));
            }
            catch (Exception ex)
            {
                Log.Error($"[PurgaLib] Disconnect/Kicked error for {hub.PlayerId}:\n{ex}");
            }
        }

        [HarmonyPatch(typeof(RoleSpawnpointManager), nameof(RoleSpawnpointManager.SetPosition))]
        [HarmonyPrefix]
        public static void Prefix_Spawning(ReferenceHub hub)
        {
            var player = PurgaLibAPI.Features.Player.Get(hub);
            if (player == null) return;

            try { PlayerHandlers.InvokeSafely(new PlayerSpawningEventArgs(player)); }
            catch (Exception ex) { Log.Error($"[PurgaLib] Spawning error:\n{ex}"); }
        }

        [HarmonyPatch(typeof(RoleSpawnpointManager), nameof(RoleSpawnpointManager.SetPosition))]
        [HarmonyPostfix]
        public static void Postfix_Spawned(ReferenceHub hub)
        {
            var player = PurgaLibAPI.Features.Player.Get(hub);
            if (player == null) return;

            try { PlayerHandlers.InvokeSafely(new PlayerSpawnedEventArgs(player)); }
            catch (Exception ex) { Log.Error($"[PurgaLib] Spawned error:\n{ex}"); }
        }

        [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.KillPlayer))]
        [HarmonyPrefix]
        public static void Prefix_Dying(PlayerStats __instance, DamageHandlerBase handler)
        {
            if (__instance == null) return;

            var targetPlayer = PurgaLibAPI.Features.Player.Get(__instance._hub);
            if (targetPlayer == null) return;
            
            var ev = new PlayerDyingEventArgs(targetPlayer, null, handler.ServerLogsText);
            try
            {
                PlayerHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Log.Error($"[PurgaLib] Dying error:\n{ex}");
            }
        }

        [HarmonyPatch(typeof(PlayerStats), nameof(PlayerStats.KillPlayer))]
        [HarmonyPostfix]
        public static void Postfix_Died(PlayerStats __instance, DamageHandlerBase handler)
        {
            if (__instance == null) return;

            var victim = PurgaLibAPI.Features.Player.Get(__instance._hub);
            if (victim == null) return;
            
            var ev = new PlayerDiedEventArgs(victim, null, handler.ServerLogsText.GetHashCode());
            try
            {
                PlayerHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Log.Error($"[PurgaLib] Died error:\n{ex}");
            }
        }
        
        [HarmonyPatch(typeof(PlayerRoleManager), nameof(PlayerRoleManager.InitializeNewRole))]
        [HarmonyPrefix]
        public static void Prefix_ChangingRole(PlayerRoleManager __instance, RoleTypeId targetId, RoleChangeReason reason)
        {
            var hub = __instance.Hub; 
            if (hub == null) return;

            var player = PurgaLibAPI.Features.Player.Get(hub);
            if (player == null) return;

            var oldRole = __instance.CurrentRole != null ? __instance.CurrentRole.RoleTypeId : RoleTypeId.None;

            var ev = new PlayerChangingRoleEventArgs(player, oldRole, targetId);
            try
            {
                PlayerHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Log.Error($"[PurgaLib] ChangingRole error:\n{ex}");
            }
        }

        [HarmonyPatch(typeof(PlayerRoleManager), nameof(PlayerRoleManager.InitializeNewRole))]
        [HarmonyPostfix]
        public static void Postfix_ChangedRole(PlayerRoleManager __instance, RoleTypeId targetId, RoleChangeReason reason)
        {
            var hub = __instance.Hub;
            if (hub == null) return;

            var player = PurgaLibAPI.Features.Player.Get(hub);
            if (player == null) return;

            var oldRole = __instance.CurrentRole != null ? __instance.CurrentRole.RoleTypeId : RoleTypeId.None;

            var ev = new PlayerChangedRoleEventArgs(player, oldRole, targetId);
            try
            {
                PlayerHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Log.Error($"[PurgaLib] ChangedRole error:\n{ex}");
            }
        }
        
        [HarmonyPatch(typeof(BanHandler), nameof(BanHandler.IssueBan))]
        [HarmonyPostfix]
        public static void Postfix_Banned(BanDetails ban, BanHandler.BanType banType, bool forced, bool __result)
        {
            if (!__result) return;

            if (!PurgaLibAPI.Features.Player.TryGet(ban.Id, out var player))
                return;

            var ev = new PlayerBannedEventArgs(player, banType);
            try
            {
                PlayerHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Log.Error($"[PurgaLib] Banned error:\n{ex}");
            }
        }

        [HarmonyPatch(typeof(RespawnTokensManager), nameof(RespawnTokensManager.OnWaveSpawned))]
        [HarmonyPostfix]
        public static void Postfix_TeamRespawned(SpawnableWaveBase wave) 
        {
            if (wave == null) return;
            
            Team team = RoleTypeExtensions.FactionToTeam(wave.TargetFaction);
            
            int count = wave.MaxWaveSize;
            var ev = new RespawnedTeamEventArgs(team, count);
            try
            {
                PlayerHandlers.InvokeSafely(ev);
            }
            catch (Exception ex)
            {
                Log.Error($"[PurgaLib] Respawned Team error:\n{ex}");
            }
        }
        
    }
}
