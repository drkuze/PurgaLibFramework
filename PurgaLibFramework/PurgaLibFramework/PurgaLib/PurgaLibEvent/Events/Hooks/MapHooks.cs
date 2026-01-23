using System;
using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handlers;
using Door = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Door;
using Elevator = PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Elevator;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Hooks
{
    [HarmonyPatch]
    public static class MapHooks
    {
        [HarmonyPatch(typeof(DoorVariant), nameof(DoorVariant.ServerInteract), typeof(ReferenceHub), typeof(byte))]
        [HarmonyPrefix]
        public static void Prefix_DoorInteracting(DoorVariant __instance, ReferenceHub ply, byte colliderId)
        {
            var player = PurgaLibAPI.Features.Player.Get(ply);
            if (player == null) return;

            var door = new Door(__instance); 
            var ev = new DoorInteractingEventArgs(player, door);
            try { MapHandlers.InvokeSafely(ev); }
            catch (Exception ex) { Log.Error($"[PurgaLib] DoorInteracting error:\n{ex}"); }
        }

        [HarmonyPatch(typeof(ElevatorDoor), nameof(ElevatorDoor.ServerInteract))]
        [HarmonyPrefix]
        public static void Prefix_ElevatorUsing(ElevatorDoor __instance, ReferenceHub ply)
        {
            var player = PurgaLibAPI.Features.Player.Get(ply);
            if (player == null) return;

            var elevator = new Elevator(__instance); 
            var ev = new ElevatorUsingEventArgs(player, elevator);
            try { MapHandlers.InvokeSafely(ev); }
            catch (Exception ex) { Log.Error($"[PurgaLib] ElevatorUsing error:\n{ex}"); }
        }

        [HarmonyPatch(typeof(TeslaGateController), nameof(TeslaGateController.ServerReceiveMessage))]
        [HarmonyPrefix]
        public static void Prefix_TeslaInteracting(NetworkConnection conn, TeslaHitMsg msg)
        {
            ReferenceHub hub;
            if (!ReferenceHub.TryGetHubNetID(conn.identity.netId, out hub)) return;
            if (hub == null || msg.Gate == null) return;

            var player = PurgaLibAPI.Features.Player.Get(hub);
            if (player == null) return;

            var ev = new OnInteractingTeslaEventArgs(player, msg.Gate);
            try 
            { 
                MapHandlers.InvokeSafely(ev); 
                if (!ev.IsAllowed)
                {
                    return;
                }
            }
            catch (Exception ex) 
            { 
                Log.Error($"[PurgaLib] TeslaInteracting error:\n{ex}"); 
            }
        }

    }
}
