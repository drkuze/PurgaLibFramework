using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mirror;
using PurgaLib.API.Enums;
using UnityEngine;

namespace PurgaLib.API.Features
{
    public static class PrefabHelper
    {
        internal static readonly Dictionary<PrefabType, (GameObject, Component)> Prefabs = new(Enum.GetValues(typeof(PrefabType)).Length);

        public static IReadOnlyDictionary<PrefabType, (GameObject, Component)> PrefabToGameObjectAndComponent => Prefabs;
        public static IReadOnlyDictionary<PrefabType, GameObject> PrefabToGameObject => Prefabs.ToDictionary(x => x.Key, x => x.Value.Item1);

        public static PrefabAttribute GetPrefabAttribute(this PrefabType prefabType)
        {
            Type type = prefabType.GetType();
            return type.GetField(Enum.GetName(type, prefabType))?.GetCustomAttribute<PrefabAttribute>();
        }

        public static GameObject GetPrefab(PrefabType prefabType)
        {
            return Prefabs.TryGetValue(prefabType, out var prefab) ? prefab.Item1 : null;
        }

        public static bool TryGetPrefab(PrefabType prefabType, out GameObject gameObject)
        {
            gameObject = GetPrefab(prefabType);
            return gameObject != null;
        }

        public static T GetPrefab<T>(PrefabType prefabType) where T : Component
        {
            return Prefabs.TryGetValue(prefabType, out var prefab) ? (T)prefab.Item2 : null;
        }

        public static GameObject Spawn(PrefabType prefabType, Vector3 position = default, Quaternion? rotation = null)
        {
            if (!TryGetPrefab(prefabType, out GameObject prefab))
            {
                prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
                prefab.transform.localScale = Vector3.one;
                prefab.GetComponent<Renderer>().material.color = Color.magenta;
            }

            GameObject obj = UnityEngine.Object.Instantiate(prefab, position, rotation ?? Quaternion.identity);
            NetworkServer.Spawn(obj);
            return obj;
        }

        public static T Spawn<T>(PrefabType prefabType, Vector3 position = default, Quaternion? rotation = null) where T : Component
        {
            GameObject obj = Spawn(prefabType, position, rotation);
            return obj?.GetComponent<T>();
        }
        
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class PrefabAttribute : Attribute
    {
        public uint Id { get; }
        public string Name { get; }
        public PrefabAttribute(uint id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

