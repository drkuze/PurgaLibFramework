using System;
using PurgaLib.API.Core.Interfaces;
using UnityEngine;

namespace PurgaLib.API.Features.Schematics.Factory
{
    public class PrimitiveFactory : ISchematicBlockFactory
    {
        public bool CanHandle(SchematicBlock block) => block.BlockType == 1;
        
        public GameObject Spawn(SchematicBlock block)
        {
            PrimitiveType type = PrimitiveType.Cube;
            if (block.Properties != null && block.Properties.TryGetValue("PrimitiveType", out var pt))
                type = (PrimitiveType)Convert.ToInt32(pt);

            var obj = GameObject.CreatePrimitive(type);
            
            ApplyTransform.ApplyCommonTransform(obj, block);
            ApplyColor.Color(obj, block);

            return obj;
        }
    }
}
