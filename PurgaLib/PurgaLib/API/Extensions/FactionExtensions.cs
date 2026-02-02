using PlayerRoles;
using PurgaLib.API.Enums;

namespace PurgaLib.API.Extensions;

public static class FactionExtensions
{
    public static bool TryGetSpawnableFaction(this Faction faction, out SpawnableFaction spawnable, bool mini = false)
    {
        spawnable = SpawnableFaction.None;

        return faction switch
        {
            Faction.FoundationStaff => SetSpawnable(ref spawnable, mini ? SpawnableFaction.MiniNtfWave : SpawnableFaction.NtfWave),
            Faction.FoundationEnemy => SetSpawnable(ref spawnable, mini ? SpawnableFaction.MiniChaosWave : SpawnableFaction.ChaosWave),
            _ => false
        };
    }

    private static bool SetSpawnable(ref SpawnableFaction target, SpawnableFaction value)
    {
        target = value;
        return true;
    }
}