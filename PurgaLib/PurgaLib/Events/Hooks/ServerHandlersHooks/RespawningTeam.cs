namespace PurgaLib.Events.Hooks.ServerHandlersHooks
{
    using HarmonyLib;
    using PlayerRoles;
    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Server;
    using PurgaLib.Events.Handlers;
    using Respawning.NamingRules;
    using Respawning.Waves;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;
    using static HarmonyLib.AccessTools;

    [HarmonyPatch(typeof(WaveSpawner), nameof(WaveSpawner.SpawnWave))]
    internal static class OnRespawningTeam
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

            int offset = -2;
            int index = newInstructions.FindIndex(i => i.Calls(Method(typeof(NamingRulesManager), nameof(NamingRulesManager.TryGetNamingRule)))) + offset;

            LocalBuilder ev = generator.DeclareLocal(typeof(RespawningTeamEventArgs));
            Label continueLabel = generator.DefineLabel();

            newInstructions.InsertRange(
                index,
                new[]
                {
                    // GetPlayers(list)
                    new CodeInstruction(OpCodes.Ldloc_S, 3).MoveLabelsFrom(newInstructions[index]),
                    new CodeInstruction(OpCodes.Call, Method(typeof(OnRespawningTeam), nameof(GetPlayers))),

                    // maxWaveSize
                    new CodeInstruction(OpCodes.Ldloc_0),

                    // wave (arg_0)
                    new CodeInstruction(OpCodes.Ldarg_0),

                    // WaveSpawner.SpawnQueue
                    new CodeInstruction(OpCodes.Ldsfld, Field(typeof(WaveSpawner), nameof(WaveSpawner.SpawnQueue))),

                    // new RespawningTeamEventArgs(players, maxWaveSize, wave, spawnQueue)
                    new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(RespawningTeamEventArgs))[0]),
                    new CodeInstruction(OpCodes.Dup),
                    new CodeInstruction(OpCodes.Stloc_S, ev.LocalIndex),

                    // ServerHandlers.InvokeSafely(ev)
                    new CodeInstruction(OpCodes.Call, Method(typeof(ServerHandlers), nameof(ServerHandlers.InvokeSafely), new[] { typeof(RespawningTeamEventArgs) })),

                    // if (ev.IsAllowed) goto continueLabel
                    new CodeInstruction(OpCodes.Ldloc_S, ev.LocalIndex),
                    new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(RespawningTeamEventArgs), nameof(RespawningTeamEventArgs.IsAllowed))),
                    new CodeInstruction(OpCodes.Brtrue_S, continueLabel),

                    // Clear queue and return empty list
                    new CodeInstruction(OpCodes.Ldsfld, Field(typeof(WaveSpawner), nameof(WaveSpawner.SpawnQueue))),
                    new CodeInstruction(OpCodes.Callvirt, Method(typeof(Queue<RoleTypeId>), nameof(Queue<RoleTypeId>.Clear))),
                    new CodeInstruction(OpCodes.Newobj, Constructor(typeof(List<ReferenceHub>), new Type[0])),
                    new CodeInstruction(OpCodes.Ret),

                    // continueLabel: apply ev modifications back
                    new CodeInstruction(OpCodes.Ldloc_S, ev.LocalIndex).WithLabels(continueLabel),
                    new CodeInstruction(OpCodes.Dup),
                    new CodeInstruction(OpCodes.Dup),
                    new CodeInstruction(OpCodes.Dup),

                    // num = ev.MaximumRespawnAmount
                    new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(RespawningTeamEventArgs), nameof(RespawningTeamEventArgs.MaximumRespawnAmount))),
                    new CodeInstruction(OpCodes.Stloc_0),

                    // list = GetHubs(ev.Players)
                    new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(RespawningTeamEventArgs), nameof(RespawningTeamEventArgs.Players))),
                    new CodeInstruction(OpCodes.Call, Method(typeof(OnRespawningTeam), nameof(GetHubs))),
                    new CodeInstruction(OpCodes.Stloc_S, 3),

                    // wave = ev.Wave
                    new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(RespawningTeamEventArgs), nameof(RespawningTeamEventArgs.Wave))),
                    new CodeInstruction(OpCodes.Starg_S, 0),

                    // WaveSpawner.SpawnQueue = ev.SpawnQueue
                    new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(RespawningTeamEventArgs), nameof(RespawningTeamEventArgs.SpawnQueue))),
                    new CodeInstruction(OpCodes.Stsfld, Field(typeof(WaveSpawner), nameof(WaveSpawner.SpawnQueue))),
                });

            offset = -3;
            index = newInstructions.FindIndex(i => i.Calls(Method(typeof(SpawnableWaveBase), nameof(SpawnableWaveBase.PopulateQueue)))) + offset;
            List<Label> extractedLabels = newInstructions[index].ExtractLabels();
            newInstructions.RemoveRange(index, 4);
            newInstructions[index].WithLabels(extractedLabels);

            foreach (CodeInstruction instruction in newInstructions)
                yield return instruction;
        }

        private static List<Player> GetPlayers(ReferenceHub[] hubs) =>
            hubs.Select(Player.Get).ToList();

        private static ReferenceHub[] GetHubs(List<Player> players) =>
            players.Select(p => p.ReferenceHub).ToArray();
    }
}