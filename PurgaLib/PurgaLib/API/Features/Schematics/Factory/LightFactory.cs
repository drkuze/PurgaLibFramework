using System;
using PurgaLib.API.Core.Interfaces;
using UnityEngine;

namespace PurgaLib.API.Features.Schematics.Factory;

public class LightFactory : ISchematicBlockFactory
{
    public bool CanHandle(SchematicBlock block) => block.BlockType == 2;

    public GameObject Spawn(SchematicBlock block)
    {
        var obj = new GameObject(block.Name);
        var light = obj.AddComponent<Light>();
        if (block.Properties != null)
        {
            if (block.Properties.TryGetValue("Color", out var c) && ColorUtility.TryParseHtmlString("#" + c.ToString(), out var col))
                light.color = col;
            if (block.Properties.TryGetValue("Intensity", out var i)) light.intensity = Convert.ToSingle(i);
            if (block.Properties.TryGetValue("Range", out var r)) light.range = Convert.ToSingle(r);
            if (block.Properties.TryGetValue("Shadows", out var s) && Convert.ToBoolean(s))
                light.shadows = LightShadows.Soft;
        }
        ApplyTransform.ApplyCommonTransform(obj, block);
        return obj;
    }
}
