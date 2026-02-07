using System;
using System.Collections.Generic;
using PurgaLib.API.Features.Server;
using UnityEngine;

namespace PurgaLib.API.Features.Schematics
{
    public static class SchematicSpawner
    {
        public static void SpawnFromFile(string path, Vector3 origin)
        {
            Logged.Info($"[SchematicSpawner] Loading schematic from file: {path}");

            if (!System.IO.File.Exists(path))
            {
                Logged.Error($"[SchematicSpawner] File does not exist: {path}");
                return;
            }

            string json;
            try
            {
                json = System.IO.File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Logged.Error($"[SchematicSpawner] Failed to read file: {e}");
                return;
            }

            SchematicRoot schematic;
            try
            {
                schematic = JsonSerialize.FromJson<SchematicRoot>(json);
                if (schematic?.Blocks == null || schematic.Blocks.Count == 0)
                {
                    Logged.Warn("[SchematicSpawner] Schematic is empty or failed to parse JSON.");
                    return;
                }

                Logged.Info($"[SchematicSpawner] Parsed schematic with {schematic.Blocks.Count} blocks.");
            }
            catch (Exception e)
            {
                Logged.Error($"[SchematicSpawner] Failed to parse JSON: {e}");
                return;
            }

            Spawn(schematic, origin);
        }

        public static void Spawn(SchematicRoot schematic, Vector3 origin)
        {
            // Assicurati di caricare tutti i prefab prima
            PrefabDatabase.LoadPrefabs();

            var map = new Dictionary<long, GameObject>();

            foreach (var block in schematic.Blocks)
            {
                try
                {
                    GameObject obj = SpawnBlock(block);

                    // Applica transform comune
                    ApplyTransform.ApplyCommonTransform(obj, block);

                    // Salva nella mappa per parenting
                    map[block.ObjectId] = obj;

                    Logged.Info($"[SchematicSpawner] Spawned block '{block.Name}' (ID: {block.ObjectId}) at {obj.transform.localPosition}");
                }
                catch (Exception e)
                {
                    Logged.Error($"[SchematicSpawner] Error spawning block '{block.Name}': {e}");
                }
            }

            // Imposta i genitori
            foreach (var block in schematic.Blocks)
            {
                try
                {
                    if (!map.TryGetValue(block.ObjectId, out var obj))
                    {
                        Logged.Warn($"[SchematicSpawner] Block ID {block.ObjectId} not found in map. Skipping parenting.");
                        continue;
                    }

                    if (map.TryGetValue(block.ParentId, out var parent))
                    {
                        obj.transform.SetParent(parent.transform, false);
                        Logged.Info($"[SchematicSpawner] Set parent of '{block.Name}' to '{parent.name}'");
                    }
                    else
                    {
                        obj.transform.SetParent(null, false);
                        obj.transform.position += origin;
                        Logged.Info($"[SchematicSpawner] '{block.Name}' has no parent. Placed at world position {obj.transform.position}");
                    }
                }
                catch (Exception e)
                {
                    Logged.Error($"[SchematicSpawner] Error setting parent for '{block.Name}': {e}");
                }
            }
        }

        private static GameObject SpawnBlock(SchematicBlock block)
        {
            switch (block.BlockType)
            {
                case 1: // Primitive
                    return SpawnPrimitive(block);

                case 2: // Light
                    return SpawnLight(block);

                case 0: // Prefab
                    return SpawnPrefab(block);

                default:
                    Logged.Warn($"[SchematicSpawner] Unknown BlockType '{block.BlockType}' for '{block.Name}'. Creating empty GameObject.");
                    return new GameObject(block.Name);
            }
        }

        private static GameObject SpawnPrimitive(SchematicBlock block)
        {
            try
            {
                PrimitiveType type = PrimitiveType.Cube;
                if (block.Properties != null && block.Properties.TryGetValue("PrimitiveType", out var typeObj))
                    type = (PrimitiveType)Convert.ToInt32(typeObj);

                var obj = GameObject.CreatePrimitive(type);
                obj.name = block.Name;

                // Colore
                ApplyColor.Color(obj, block);

                return obj;
            }
            catch (Exception e)
            {
                Logged.Error($"[SchematicSpawner] Failed to spawn primitive '{block.Name}': {e}");
                return new GameObject(block.Name);
            }
        }

        private static GameObject SpawnLight(SchematicBlock block)
        {
            try
            {
                var obj = new GameObject(block.Name);
                var light = obj.AddComponent<Light>();

                if (block.Properties != null)
                {
                    if (block.Properties.TryGetValue("Color", out var colObj))
                        light.color = ParseColor(colObj.ToString());

                    if (block.Properties.TryGetValue("Intensity", out var intObj))
                        light.intensity = Convert.ToSingle(intObj);

                    if (block.Properties.TryGetValue("Range", out var rangeObj))
                        light.range = Convert.ToSingle(rangeObj);

                    if (block.Properties.TryGetValue("Shadows", out var shadowObj))
                        light.shadows = Convert.ToBoolean(shadowObj) ? LightShadows.Soft : LightShadows.None;
                }

                return obj;
            }
            catch (Exception e)
            {
                Logged.Error($"[SchematicSpawner] Failed to spawn light '{block.Name}': {e}");
                return new GameObject(block.Name);
            }
        }

        private static GameObject SpawnPrefab(SchematicBlock block)
        {
            try
            {
                string prefabName = block.Properties?["Prefab"]?.ToString() ?? block.Name;

                var prefab = PrefabDatabase.Get(prefabName);
                if (prefab != null)
                {
                    var obj = UnityEngine.Object.Instantiate(prefab);
                    obj.name = block.Name;
                    Logged.Info($"[SchematicSpawner] Spawned prefab '{prefabName}' as '{block.Name}'");
                    return obj;
                }

                Logged.Warn($"[SchematicSpawner] Prefab '{prefabName}' not found. Creating empty GameObject.");
                return new GameObject(block.Name);
            }
            catch (Exception e)
            {
                Logged.Error($"[SchematicSpawner] Failed to spawn prefab '{block.Name}': {e}");
                return new GameObject(block.Name);
            }
        }

        private static Color ParseColor(string hex)
        {
            if (!hex.StartsWith("#")) hex = "#" + hex;
            if (ColorUtility.TryParseHtmlString(hex, out var c))
                return c;

            Logged.Warn($"[SchematicSpawner] Failed to parse color '{hex}', using white.");
            return Color.white;
        }
    }
}
