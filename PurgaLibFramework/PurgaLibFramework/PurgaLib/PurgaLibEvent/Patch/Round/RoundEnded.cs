using HarmonyLib;
using PlayerRoles;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Round;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler;

namespace PurgaLibEvents.PurgaLibEvent.Patch.Round;

[PurgaLibEventPatcher(typeof(RoundHandler), nameof(RoundHandler.OnEnded))]
[HarmonyPatch(typeof(RoundSummary), nameof(RoundSummary.RoundEnded))]
public static class RoundEndedPatch
{
    private static void Prefix(Team team)
    {
        RoundHandler.OnEnded(new RoundEndedEventArgs(team.ToString()));
    }
}