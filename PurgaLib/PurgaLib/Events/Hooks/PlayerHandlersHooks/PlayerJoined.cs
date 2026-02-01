namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using CentralAuth;
    using HarmonyLib;
    using Mirror;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    [HarmonyPatch(typeof(PlayerAuthenticationManager), nameof(PlayerAuthenticationManager.FinalizeAuthentication))]
    internal static class PlayerJoined
    {
        internal static void OnPlayerJoined(ReferenceHub hub)
        {
            if (!NetworkServer.active)
                return;

            Player player = Player.Get(hub);

            if (player == null)
            {
                Logged.Warn($"[PurgaLib] Player not found for ReferenceHub {hub.PlayerId}");
                return;
            }

            if (player.IsHost)
                return;

            try
            {
                PlayerHandlers.InvokeSafely(new PlayerJoinedEventArgs(player));
            }
            catch (Exception ex)
            {
                Logged.Error($"[PurgaLib] PlayerJoined event error for PlayerID {hub.PlayerId}:\n{ex}");
            }
        }

        private static void Postfix(PlayerAuthenticationManager __instance)
        {
            OnPlayerJoined(__instance._hub);
        }
    }

    [HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.UserCode_CmdSetNick__String))]
    internal static class PlayerJoinedOfflineMode
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

            int index = -1;
            for (int i = 0; i < newInstructions.Count; i++)
            {
                if (newInstructions[i].opcode == OpCodes.Callvirt &&
                    newInstructions[i].operand is MethodInfo method &&
                    method.Name == nameof(CharacterClassManager.SyncServerCmdBinding))
                {
                    index = i + 1;
                    break;
                }
            }

            if (index > 0)
            {
                newInstructions.InsertRange(
                    index,
                    new[]
                    {
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(NicknameSync), nameof(NicknameSync._hub))),
                        new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PlayerJoined), nameof(PlayerJoined.OnPlayerJoined))),
                    });
            }

            foreach (var instruction in newInstructions)
                yield return instruction;
        }
    }
}
