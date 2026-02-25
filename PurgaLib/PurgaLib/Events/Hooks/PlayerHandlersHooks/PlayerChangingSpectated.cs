namespace PurgaLib.Events.Hooks.PlayerHandlersHooks
{
    #pragma warning disable SA1402
    #pragma warning disable SA1313
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using HarmonyLib;

    using PlayerRoles;
    using PlayerRoles.Spectating;

    using PurgaLib.API.Features;
    using PurgaLib.API.Features.Server;
    using PurgaLib.Events.EventArgs.Player;
    using PurgaLib.Events.Handlers;

    using static HarmonyLib.AccessTools;

    /// <summary>
    /// Patches the <see cref="SpectatorRole.SyncedSpectatedNetId"/> setter.
    /// Adds the <see cref="PlayerHandlers.ChangingSpectated"/> event.
    /// </summary>
    [HarmonyPatch(typeof(SpectatorRole), nameof(SpectatorRole.SyncedSpectatedNetId), MethodType.Setter)]
    internal static class PlayerChangingSpectated
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = new List<CodeInstruction>(instructions);

            Label retLabel = generator.DefineLabel();
            LocalBuilder ownerLocal = generator.DeclareLocal(typeof(ReferenceHub));
            LocalBuilder evLocal = generator.DeclareLocal(typeof(PlayerChangingSpectatedEventArgs));

            newInstructions.InsertRange(0, new CodeInstruction[]
            {
                // this.TryGetOwner(out ReferenceHub owner)
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldloca_S, ownerLocal.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(PlayerRoleBase), nameof(PlayerRoleBase.TryGetOwner), new[] { typeof(ReferenceHub).MakeByRefType() })),
                new CodeInstruction(OpCodes.Pop), // discard bool

                // PlayerChangingSpectated.CreateAndFire(owner, this.SyncedSpectatedNetId, value)
                new CodeInstruction(OpCodes.Ldloc_S, ownerLocal.LocalIndex),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(SpectatorRole), nameof(SpectatorRole.SyncedSpectatedNetId))),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, Method(typeof(PlayerChangingSpectated), nameof(CreateAndFire))),
                new CodeInstruction(OpCodes.Stloc_S, evLocal.LocalIndex),

                // if (!ev.IsAllowed) return;
                new CodeInstruction(OpCodes.Ldloc_S, evLocal.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(PlayerChangingSpectatedEventArgs), nameof(PlayerChangingSpectatedEventArgs.IsAllowed))),
                new CodeInstruction(OpCodes.Brtrue_S, retLabel),

                new CodeInstruction(OpCodes.Ret),
            });

            newInstructions[14].WithLabels(retLabel);

            foreach (CodeInstruction instruction in newInstructions)
                yield return instruction;
        }

        private static PlayerChangingSpectatedEventArgs CreateAndFire(ReferenceHub ownerHub, uint oldNetId, uint newNetId)
        {
            Player spectator = Player.Get(ownerHub);
            Player oldTarget = GetPlayerByNetId(oldNetId);
            Player newTarget = GetPlayerByNetId(newNetId);

            var ev = new PlayerChangingSpectatedEventArgs(spectator, oldTarget, newTarget);
            PlayerHandlers.InvokeSafely(ev);
            return ev;
        }

        private static Player GetPlayerByNetId(uint netId)
        {
            if (netId == 0)
                return null;

            foreach (ReferenceHub hub in ReferenceHub.AllHubs)
            {
                if (hub.netId == netId)
                    return Player.Get(hub);
            }

            return null;
        }
    }
}