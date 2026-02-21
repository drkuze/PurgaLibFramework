namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using HarmonyLib;
    using Interactables.Interobjects;
    using Interactables.Interobjects.DoorUtils;

    using PurgaLib.API.Features;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="EmergencyDoorRelease.ServerInteract(ReferenceHub, byte)"/>.
    /// Adds the <see cref="PlayerHandlers.InteractingEmergencyButton"/> event.
    /// </summary>
    [HarmonyPatch(typeof(EmergencyDoorRelease), nameof(EmergencyDoorRelease.ServerInteract), typeof(ReferenceHub), typeof(byte))]
    internal static class PlayerInteractingEmergencyButton
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

            Label transpilerLabel = generator.DefineLabel();

            // Find the last Brtrue_S — same logic as Exiled
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Brtrue_S) + 2;

            // Save the original jump target and redirect it to our new block
            Label continueLabel = (Label)newInstructions[index - 2].operand;
            newInstructions[index - 2].operand = transpilerLabel;

            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                // this (EmergencyDoorRelease)
                new CodeInstruction(OpCodes.Ldarg_0).WithLabels(transpilerLabel),

                // this._controlledDoor (DoorVariant)
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(EmergencyDoorRelease), nameof(EmergencyDoorRelease._controlledDoor))),

                // Player.Get(referenceHub)
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),

                // new PlayerInteractingEmergencyButtonEventArgs(emergencyDoorRelease, door, player)
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(PlayerInteractingEmergencyButtonEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),

                // PlayerHandlers.InvokeSafely(ev)
                new CodeInstruction(OpCodes.Call, Method(typeof(PlayerHandlers), nameof(PlayerHandlers.InvokeSafely), new[] { typeof(PlayerInteractingEmergencyButtonEventArgs) })),

                // if (ev.IsAllowed) goto continueLabel;
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(PlayerInteractingEmergencyButtonEventArgs), nameof(PlayerInteractingEmergencyButtonEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brtrue_S, continueLabel),

                // else return;
                new CodeInstruction(OpCodes.Ret),
            });

            foreach (CodeInstruction instruction in newInstructions)
                yield return instruction;
        }
    }
}