namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using HarmonyLib;
    using Mirror;

    using PlayerRoles.Voice;

    using API.Features;
    using API.Features.Server;
    using Events.EventArgs.Player;
    using Events.Handlers;

    using VoiceChat;
    using VoiceChat.Networking;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches <see cref="VoiceTransceiver.ServerReceiveMessage(NetworkConnection, VoiceMessage)"/>.
    /// Adds the <see cref="PlayerHandlers.VoiceChatting"/> event.
    /// </summary>
    [HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    internal static class PlayerVoiceChatting
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);
            Label retLabel = generator.DefineLabel();
            LocalBuilder evLocal = generator.DeclareLocal(typeof(PlayerVoiceChattingEventArgs));

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldfld, Field(typeof(VoiceMessage), nameof(VoiceMessage.Speaker))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(ReferenceHub) })),

                new CodeInstruction(OpCodes.Ldarg_1),

                new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(PlayerVoiceChattingEventArgs))[0]),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Stloc_S, evLocal.LocalIndex),

                new CodeInstruction(OpCodes.Call, Method(typeof(PlayerHandlers), nameof(PlayerHandlers.InvokeSafely), new[] { typeof(PlayerVoiceChattingEventArgs) })),

                new CodeInstruction(OpCodes.Ldloc_S, evLocal.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(PlayerVoiceChattingEventArgs), nameof(PlayerVoiceChattingEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brfalse_S, retLabel),

                new CodeInstruction(OpCodes.Ldloc_S, evLocal.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(PlayerVoiceChattingEventArgs), nameof(PlayerVoiceChattingEventArgs.VoiceMessage))),
                new CodeInstruction(OpCodes.Starg_S, 1),
            });

            newInstructions[newInstructions.Count - 1].WithLabels(retLabel);

            foreach (CodeInstruction instruction in newInstructions)
                yield return instruction;
        }
    }
}