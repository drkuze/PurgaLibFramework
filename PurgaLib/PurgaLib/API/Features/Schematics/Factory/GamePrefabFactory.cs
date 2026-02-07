using PurgaLib.API.Core.Interfaces;
using UnityEngine;

namespace PurgaLib.API.Features.Schematics.Factory;

public class GamePrefabFactory : ISchematicBlockFactory
{
    public bool CanHandle(SchematicBlock block)
        => block.Properties != null && block.Properties.ContainsKey("Prefab");

    public GameObject Spawn(SchematicBlock block)
    {
        string prefabName = block.Properties["Prefab"].ToString();
        var prefab = PrefabDatabase.Get(prefabName); 
        if (prefab == null) return new GameObject(block.Name);

        var obj = Object.Instantiate(prefab);
        obj.name = block.Name;
        ApplyTransform.ApplyCommonTransform(obj, block);
        return obj;
    }
}
