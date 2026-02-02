using PurgaLib.API.Enums;
using Respawning.Waves;

namespace PurgaLib.API.Extensions
{
    public static class SpawnableWaveExtensions
    {
        public static SpawnableFaction GetSpawnableFaction(this SpawnableWaveBase wave)
        {
            if (wave is NtfSpawnWave) return SpawnableFaction.NtfWave;
            if (wave is NtfMiniWave) return SpawnableFaction.MiniNtfWave;
            if (wave is ChaosSpawnWave) return SpawnableFaction.ChaosWave;
            if (wave is ChaosMiniWave) return SpawnableFaction.MiniChaosWave;

            return SpawnableFaction.None;
        }
    }
}
