namespace PurgaLib.Events.Hooks.SCPsHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using Interactables.Interobjects.DoorUtils;
    using PurgaLib.API.Features;
    using PurgaLib.Events.EventArgs.SCP;
    using PurgaLib.Events.Handlers;
    using Scp914;

    [HarmonyPatch(typeof(Scp914Upgrader), nameof(Scp914Upgrader.ProcessPlayer))]
    public static class Scp914Upgrading
    {
        public static bool Prefix(
            ReferenceHub ply,
            bool upgradeInventory,
            bool heldOnly,
            Scp914KnobSetting setting)
        {
            var player = new Player(ply);
            var ev = new UpgradingPlayersEventArgs(player, setting);

            SCPsHandlers.InvokeSafely(ev);

            return ev.IsAllowed;
        }
    }
}