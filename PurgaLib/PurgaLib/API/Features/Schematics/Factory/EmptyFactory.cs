using PurgaLib.API.Core.Interfaces;
using UnityEngine;

namespace PurgaLib.API.Features.Schematics.Factory;

public class EmptyFactory : ISchematicBlockFactory
{
    public bool CanHandle(SchematicBlock block) => block.BlockType == 0;
    public GameObject Spawn(SchematicBlock block)
    {
        var obj = new GameObject(block.Name);
        ApplyTransform.ApplyCommonTransform(obj, block);
        return obj;
    }
}
