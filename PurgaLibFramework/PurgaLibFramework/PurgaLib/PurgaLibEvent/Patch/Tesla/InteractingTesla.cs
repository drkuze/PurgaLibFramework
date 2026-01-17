using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Map;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Patch.Tesla
{
    [PurgaLibEventPatcher(typeof(TeslaHandler), nameof(TeslaHandler.OnInteractingTesla))]
    [HarmonyPatch(typeof(TeslaGateController), nameof(TeslaGateController.FixedUpdate))]
    public static class InteractingTesla
    {
        private static readonly FieldInfo TargetsField = 
            AccessTools.Field(typeof(TeslaGateController), "_targets");

        private static void Prefix(TeslaGateController __instance)
        {
            try
            {
                if (!__instance.enabled || TargetsField == null) 
                    return;
                
                var targets = TargetsField.GetValue(__instance) as List<ReferenceHub>;
                if (targets == null) return;

                foreach (var hub in targets)
                {
                    if (hub == null) continue;

                    var player = new PurgaLibAPI.Features.Player(hub);

                    TeslaHandler.OnInteractingTesla(
                        new OnInteractingTeslaEventArgs(player, __instance)
                    );
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error in InteractingTesla Prefix: {e}");
            }
        }
    }
}
