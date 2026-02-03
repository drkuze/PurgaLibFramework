namespace PurgaLib.Events.Hooks.MapHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using Interactables.Interobjects.DoorUtils;
    using PurgaLib.API.Features;
    using PurgaLib.Events.EventArgs.Map;
    using Door = API.Features.Door;
    using PurgaLib.Events.Handlers;
    using System;
    using PurgaLib.API.Features.Server;

    [HarmonyPatch(typeof(DoorVariant), nameof(DoorVariant.ServerInteract), typeof(ReferenceHub), typeof(byte))]
    public static class DoorInteracting
    {
        public static void Prefix(DoorVariant __instance, ReferenceHub ply, byte colliderId)
        {
            var player = Player.Get(ply);
            if (player == null) return;

            var door = new Door(__instance);
            var ev = new DoorInteractingEventArgs(player, door);
            try { MapHandlers.InvokeSafely(ev); }
            catch (Exception ex) { Logged.Error($"[PurgaLib] DoorInteracting error:\n{ex}"); }
        }
    }
}