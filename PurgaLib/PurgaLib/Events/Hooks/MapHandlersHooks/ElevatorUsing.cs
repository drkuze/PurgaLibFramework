namespace PurgaLib.Events.Hooks.MapHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using Interactables.Interobjects;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Map;
    using PurgaLib.Events.Handlers;
    using System;
    using Elevator = API.Features.Elevator;

    [HarmonyPatch(typeof(ElevatorDoor), nameof(ElevatorDoor.ServerInteract))]
    public static class ElevatorUsing
    {
        public static void Prefix(ElevatorDoor __instance, ReferenceHub ply)
        {
            var player = Player.Get(ply);
            if (player == null) return;

            var elevator = new Elevator(__instance);
            var ev = new ElevatorUsingEventArgs(player, elevator);
            try { MapHandlers.InvokeSafely(ev); }
            catch (Exception ex) { Logged.Error($"[PurgaLib] ElevatorUsing error:\n{ex}"); }
        }
    }
}