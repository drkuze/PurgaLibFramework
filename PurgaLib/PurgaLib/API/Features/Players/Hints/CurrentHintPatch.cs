using HarmonyLib;
using Hints;
using MEC;
using System.Collections.Generic;

namespace PurgaLib.API.Features.Players.Hints
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(HintDisplay), nameof(HintDisplay.Show))]
    public static class CurrentHintPatch
    {
        private static readonly Dictionary<Player, CoroutineHandle> PlayerHints = new();

        [HarmonyPostfix]
        private static void Postfix(HintDisplay __instance, Hint hint)
        {
            if (__instance == null || hint is not TextHint textHint)
                return;

            Player player = Player.Get(__instance.gameObject);
            if (player == null)
                return;

            if (PlayerHints.TryGetValue(player, out var old))
                Timing.KillCoroutines(old);

            player.CurrentHint = new PlyHint(textHint.Text, textHint.DurationScalar);

            PlayerHints[player] =
                Timing.RunCoroutine(RemoveHint(player, textHint.DurationScalar));
        }

        private static IEnumerator<float> RemoveHint(Player player, float duration)
        {
            yield return Timing.WaitForSeconds(duration);

            if (!player.IsConnected)
                yield break;

            player.CurrentHint = null;
            PlayerHints.Remove(player);
        }
    }
}