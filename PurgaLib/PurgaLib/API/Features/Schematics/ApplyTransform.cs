using UnityEngine;

namespace PurgaLib.API.Features.Schematics
{
    public static class ApplyTransform
    {
        public static void ApplyCommonTransform(GameObject obj, SchematicBlock block)
        {
            if (block.Position != null)
                obj.transform.localPosition = block.Position.Value;
            
            if (block.Rotation != null)
                obj.transform.localRotation = block.Rotation.Value;
            
            if (block.Scale != null)
                obj.transform.localScale = block.Scale.Value;
        }
    }
}
