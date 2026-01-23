using HarmonyLib;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.SCP;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handlers;
using Scp914;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Hooks
{
    [HarmonyPatch(typeof(Scp914Upgrader), nameof(Scp914Upgrader.ProcessPlayer))]
    public static class Scp914Hooks
    {
        [HarmonyPrefix]
        public static bool Prefix_ProcessPlayer(
            ReferenceHub ply,
            bool upgradeInventory,
            bool heldOnly,
            Scp914KnobSetting setting)
        {
            var player = new Player(ply);
            var ev = new UpgradingPlayersEventArgs(player, setting);
            
            SCPHandlers.InvokeSafely(ev);
            
            return ev.IsAllowed;
        }

    }
}
