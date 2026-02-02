namespace PurgaLib.Events.Hooks.MapHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using Interactables.Interobjects.DoorUtils;
    using PurgaLib.Events.EventArgs.Map;
    using PurgaLib.Events.Handlers;

    [HarmonyPatch(typeof(DoorVariant), nameof(DoorVariant.ServerInteract))]
    public static class DoorInteracting
    {
        [HarmonyPrefix]
        public static bool Prefix(DoorVariant __instance, ReferenceHub ply, byte colliderId)
        {
            var player = API.Features.Player.Get(ply);
            if (player == null)
                return true;

            var door = API.Features.Door.Get(__instance);
            if (door == null)
                return true;

            var ev = new DoorInteractingEventArgs(player, door);
            MapHandlers.InvokeSafely(ev);

            return ev.IsAllowed;
        }
    }
}