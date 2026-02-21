namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using HarmonyLib;
    using Interactables.Interobjects;
    using PurgaLib.API.Features;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="ElevatorDoor.ServerInteract"/>.
    /// Adds the <see cref="PlayerHandlers.InteractingElevator"/> event.
    /// <remarks>This event only gets called when interacting on the outside button.</remarks>
    /// </summary>
    [HarmonyPatch(typeof(ElevatorDoor), nameof(ElevatorDoor.ServerInteract))]
    internal static class PlayerInteractingElevator
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

            Label returnLabel = generator.DefineLabel();

            newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                // Player.Get(referenceHub)
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),

                // this.Chamber
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(ElevatorDoor), nameof(ElevatorDoor.Chamber))),

                // isInside = false (outside button)
                new CodeInstruction(OpCodes.Ldc_I4_0),

                // new PlayerInteractingElevatorEventArgs(player, chamber, isInside)
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(PlayerInteractingElevatorEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),

                // PlayerHandlers.InvokeSafely(ev)
                new CodeInstruction(OpCodes.Call, Method(typeof(PlayerHandlers), nameof(PlayerHandlers.InvokeSafely), new[] { typeof(PlayerInteractingElevatorEventArgs) })),

                // if (!ev.IsAllowed) return;
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(PlayerInteractingElevatorEventArgs), nameof(PlayerInteractingElevatorEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, returnLabel),
            });

            foreach (CodeInstruction instruction in newInstructions)
                yield return instruction;
        }
    }

    /// <summary>
    /// Patches <see cref="ElevatorChamber.ServerInteract"/>.
    /// Adds the <see cref="PlayerHandlers.InteractingElevator"/> event.
    /// <remarks>This event only gets called when interacting on the inside button.</remarks>
    /// </summary>
    [HarmonyPatch(typeof(ElevatorChamber), nameof(ElevatorChamber.ServerInteract))]
    internal static class PlayerInteractingElevator2
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

            Label returnLabel = generator.DefineLabel();

            newInstructions[newInstructions.Count - 1].labels.Add(returnLabel);

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                // Player.Get(referenceHub)
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),

                // this (ElevatorChamber)
                new CodeInstruction(OpCodes.Ldarg_0),

                // isInside = true (inside button)
                new CodeInstruction(OpCodes.Ldc_I4_1),

                // new PlayerInteractingElevatorEventArgs(player, chamber, isInside)
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(PlayerInteractingElevatorEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),

                // PlayerHandlers.InvokeSafely(ev)
                new CodeInstruction(OpCodes.Call, Method(typeof(PlayerHandlers), nameof(PlayerHandlers.InvokeSafely), new[] { typeof(PlayerInteractingElevatorEventArgs) })),

                // if (!ev.IsAllowed) return;
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(PlayerInteractingElevatorEventArgs), nameof(PlayerInteractingElevatorEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, returnLabel),
            });

            foreach (CodeInstruction instruction in newInstructions)
                yield return instruction;
        }
    }
}