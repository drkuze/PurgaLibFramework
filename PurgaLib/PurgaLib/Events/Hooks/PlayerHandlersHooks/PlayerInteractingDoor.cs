namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using HarmonyLib;
    using Interactables.Interobjects.DoorUtils;

    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="DoorVariant.ServerInteract(ReferenceHub, byte)"/>.
    /// Adds the <see cref="PlayerHandlers.InteractingDoor"/> event.
    /// </summary>
    [HarmonyPatch(typeof(DoorVariant), nameof(DoorVariant.ServerInteract), typeof(ReferenceHub), typeof(byte))]
    internal static class PlayerInteractingDoor
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

            Label retLabel = generator.DefineLabel();
            LocalBuilder evLocal = generator.DeclareLocal(typeof(PlayerInteractingDoorEventArgs));

            // Insert at the very beginning of the method:
            // var ev = new PlayerInteractingDoorEventArgs(Player.Get(ply), doorVariant, colliderId);
            // PlayerHandlers.InvokeSafely(ev);
            // if (!ev.IsAllowed) return;
            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                // Player.Get(__instance's hub) — arg_0 = ReferenceHub ply (first param after static patch: arg_0)
                // DoorVariant.ServerInteract(ReferenceHub ply, byte colliderId)
                // arg_0 = ReferenceHub ply, arg_1 = byte colliderId  (static method; 'this' DoorVariant is not arg_0 here — it's an instance method so: arg_0=this, arg_1=ply, arg_2=colliderId)

                // Player.Get(ply)  → arg_1
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),

                // door (this) → arg_0
                new CodeInstruction(OpCodes.Ldarg_0),

                // colliderId → arg_2
                new CodeInstruction(OpCodes.Ldarg_2),

                // new PlayerInteractingDoorEventArgs(player, door, colliderId)
                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(PlayerInteractingDoorEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Stloc_S, evLocal.LocalIndex),

                // PlayerHandlers.InvokeSafely(ev)
                new CodeInstruction(OpCodes.Call, Method(typeof(PlayerHandlers), nameof(PlayerHandlers.InvokeSafely), new[] { typeof(PlayerInteractingDoorEventArgs) })),

                // if (!ev.IsAllowed) return;
                new CodeInstruction(OpCodes.Ldloc_S, evLocal.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(PlayerInteractingDoorEventArgs), nameof(PlayerInteractingDoorEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brtrue_S, retLabel),

                new CodeInstruction(OpCodes.Ret),
            });

            newInstructions[11].WithLabels(retLabel);

            foreach (CodeInstruction instruction in newInstructions)
                yield return instruction;
        }
    }
}