using UnityEngine;

namespace PurgaLib.API.Features.Schematics
{
    public static class ApplyColor
    {
        public static void Color(GameObject obj, SchematicBlock block)
        {
            if (block.Properties != null && block.Properties.TryGetValue("Color", out var col))
            {
                if (ColorUtility.TryParseHtmlString("#" + col.ToString(), out var color))
                {
                    var renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = color;
                    }
                }
            }
        }
    }
}
