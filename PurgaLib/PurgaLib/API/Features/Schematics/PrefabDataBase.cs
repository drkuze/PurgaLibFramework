using UnityEngine;
using System.Collections.Generic;
using PurgaLib.API.Features.Server;

namespace PurgaLib.API.Features.Schematics
{
    public static class PrefabDatabase
    {
        private static readonly Dictionary<string, GameObject> _prefabs = new();
        private static bool _initialized = false;
        
        public static void LoadPrefabs()
        {
            if (_initialized) return;

            Logged.Info("[PrefabDatabase] Loading prefabs from Resources/Prefabs...");

            var loadedPrefabs = Resources.LoadAll<GameObject>("Prefabs/");
            if (loadedPrefabs.Length == 0)
            {
                Logged.Warn("[PrefabDatabase] Nessun prefab trovato in Resources/Prefabs!");
            }

            foreach (var prefab in loadedPrefabs)
            {
                if (!_prefabs.ContainsKey(prefab.name))
                {
                    _prefabs.Add(prefab.name, prefab);
                    Logged.Info($"[PrefabDatabase] Loaded prefab: {prefab.name}");
                }
                else
                {
                    Logged.Warn($"[PrefabDatabase] Prefab duplicato ignorato: {prefab.name}");
                }
            }

            _initialized = true;
            Logged.Success($"[PrefabDatabase] Totale prefab caricati: {_prefabs.Count}");
        }

        public static GameObject Get(string prefabName)
        {
            if (!_initialized)
            {
                Logged.Info("[PrefabDatabase] Prefabs non inizializzati, caricamento automatico...");
                LoadPrefabs();
            }

            if (_prefabs.TryGetValue(prefabName, out var prefab))
                return prefab;

            Logged.Warn($"[PrefabDatabase] Prefab '{prefabName}' non trovato!");
            return null;
        }
        
        public static GameObject Instantiate(string prefabName)
        {
            var prefab = Get(prefabName);
            if (prefab == null)
                return null;

            return Object.Instantiate(prefab);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            LoadPrefabs();
        }
    }
}
