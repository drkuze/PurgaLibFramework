using System;
using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using PlayerRoles;
using PurgaLib.API.Enums;
using PurgaLib.API.Extensions;
using Respawning;
using Respawning.Waves;
using Respawning.Waves.Generic;
using UnityEngine;

namespace PurgaLib.API.Features
{
    public static class Respawn
    {
        private static GameObject ntfHelicopter;
        private static GameObject chaosVan;
        
        public static List<SpawnableWaveBase> PausedWaves { get; } = new();

        public static Dictionary<Faction, float> FactionInfluence => FactionInfluenceManager.Influence;

        public static GameObject NtfHelicopter
        {
            get
            {
                if (ntfHelicopter == null)
                    ntfHelicopter = GameObject.Find("Chopper");
                return ntfHelicopter;
            }
        }

        public static GameObject ChaosVan
        {
            get
            {
                if (chaosVan == null)
                    chaosVan = GameObject.Find("CIVanArrive");
                return chaosVan;
            }
        }

        public static SpawnableFaction NextKnownSpawnableFaction =>
            WaveManager._nextWave != null ? WaveManager._nextWave.GetSpawnableFaction() : SpawnableFaction.None;

        public static WaveQueueState CurrentState => WaveManager.State;
        public static bool IsSpawning => WaveManager.State == WaveQueueState.WaveSpawning;

        public static bool ProtectionEnabled
        {
            get => SpawnProtected.IsProtectionEnabled;
            set => SpawnProtected.IsProtectionEnabled = value;
        }

        public static float ProtectionTime
        {
            get => SpawnProtected.SpawnDuration;
            set => SpawnProtected.SpawnDuration = value;
        }

        public static bool ProtectedCanShoot
        {
            get => SpawnProtected.CanShoot;
            set => SpawnProtected.CanShoot = value;
        }

        public static List<Team> ProtectedTeams => SpawnProtected.ProtectedTeams;
        
        public static bool TryGetWaveBase<T>(out T wave) where T : SpawnableWaveBase =>
            WaveManager.TryGet(out wave);

        public static bool TryGetWaveBase(SpawnableFaction faction, out SpawnableWaveBase wave)
        {
            wave = WaveManager.Waves.Find(x => x.GetSpawnableFaction() == faction);
            return wave != null;
        }

        public static bool TryGetWaveBases(Faction faction, out IEnumerable<SpawnableWaveBase> waves)
        {
            var list = WaveManager.Waves.Where(x => x.TargetFaction == faction).ToList();
            if (list.Count == 0)
            {
                waves = null;
                return false;
            }

            waves = list;
            return true;
        }
        
        public static void AdvanceTimer(Faction faction, float seconds) =>
            WaveManager.AdvanceTimer(faction, seconds);

        public static void AdvanceTimer(Faction faction, TimeSpan time) =>
            AdvanceTimer(faction, (float)time.TotalSeconds);

        public static void AdvanceTimer(SpawnableFaction faction, float seconds)
        {
            foreach (var wave in WaveManager.Waves.OfType<TimeBasedWave>())
            {
                if (wave.GetSpawnableFaction() == faction)
                    wave.Timer.AddTime(Mathf.Abs(seconds));
            }
        }

        public static void AdvanceTimer(SpawnableFaction faction, TimeSpan time) =>
            AdvanceTimer(faction, (float)time.TotalSeconds);
        
        public static void PlayEffect(SpawnableWaveBase wave)
        {
            if (wave != null)
                WaveUpdateMessage.ServerSendUpdate(wave, UpdateMessageFlags.Trigger);
        }

        public static void SummonNtfChopper()
        {
            if (TryGetWaveBase(SpawnableFaction.NtfWave, out var wave))
                PlayEffect(wave);
        }

        public static void SummonChaosInsurgencyVan()
        {
            if (TryGetWaveBase(SpawnableFaction.ChaosWave, out var wave))
                PlayEffect(wave);
        }
        
        public static bool GrantTokens(Faction faction, int amount)
        {
            if (TryGetWaveBases(faction, out var waves))
            {
                foreach (var w in waves.OfType<ILimitedWave>())
                    w.RespawnTokens += amount;
                return true;
            }
            return false;
        }

        public static bool RemoveTokens(Faction faction, int amount)
        {
            if (TryGetWaveBases(faction, out var waves))
            {
                foreach (var w in waves.OfType<ILimitedWave>())
                    w.RespawnTokens = Math.Max(0, w.RespawnTokens - amount);
                return true;
            }
            return false;
        }

        public static bool SetTokens(Faction faction, int amount)
        {
            if (TryGetWaveBases(faction, out var waves))
            {
                foreach (var w in waves.OfType<ILimitedWave>())
                    w.RespawnTokens = amount;
                return true;
            }
            return false;
        }

        public static bool TryGetTokens(SpawnableFaction faction, out int tokens)
        {
            tokens = 0;
            if (TryGetWaveBase(faction, out var wave) && wave is ILimitedWave lw)
            {
                tokens = lw.RespawnTokens;
                return true;
            }
            return false;
        }
        
        public static void GrantInfluence(Faction faction, int amount) => FactionInfluenceManager.Add(faction, amount);
        public static void RemoveInfluence(Faction faction, int amount) => FactionInfluenceManager.Remove(faction, amount);
        public static void SetInfluence(Faction faction, float value) => FactionInfluenceManager.Set(faction, value);
        public static float GetInfluence(Faction faction) => FactionInfluenceManager.Get(faction);
        public static bool TryGetFactionInfluence(Faction faction, out float value) => FactionInfluence.TryGetValue(faction, out value);
        
        public static void ForceWave(Faction faction, bool isMini = false)
        {
            if (faction.TryGetSpawnableFaction(out var spawnableFaction, isMini))
                ForceWave(spawnableFaction);
        }

        public static void ForceWave(SpawnableFaction faction)
        {
            if (TryGetWaveBase(faction, out var wave))
                ForceWave(wave);
        }

        public static void ForceWave(SpawnableWaveBase wave)
        {
            if (wave != null)
                WaveManager.Spawn(wave);
        }
        
        public static void PauseWave(SpawnableFaction faction)
        {
            if (TryGetWaveBase(faction, out var wave))
            {
                if (!PausedWaves.Contains(wave)) PausedWaves.Add(wave);
                WaveManager.Waves.Remove(wave);
            }
        }

        public static void PauseWaves()
        {
            PausedWaves.Clear();
            PausedWaves.AddRange(WaveManager.Waves);
            WaveManager.Waves.Clear();
        }

        public static void PauseWaves(List<SpawnableFaction> factions)
        {
            foreach (var f in factions)
                PauseWave(f);
        }

        public static void ResumeWaves()
        {
            WaveManager.Waves.Clear();
            WaveManager.Waves.AddRange(PausedWaves);
            PausedWaves.Clear();
        }

        public static void RestartWave(SpawnableFaction faction)
        {
            if (TryGetWaveBase(faction, out var wave))
            {
                if (!WaveManager.Waves.Contains(wave)) WaveManager.Waves.Add(wave);
                PausedWaves.Remove(wave);
            }
        }

        public static void RestartWaves()
        {
            WaveManager.Waves.Clear();
            WaveManager.Waves.AddRange(new SpawnableWaveBase[]
            {
                new ChaosMiniWave(),
                new ChaosSpawnWave(),
                new NtfMiniWave(),
                new NtfSpawnWave()
            });
            PausedWaves.Clear();
        }

        public static void RestartWaves(List<SpawnableFaction> factions)
        {
            foreach (var f in factions)
                RestartWave(f);
        }
    }
}
