using System.Collections.Generic;
using UnityEngine;

namespace PurgaLib.API.Features.Schematics
{
    public class SchematicBlock
    {
        public int BlockType { get; set; } 
        public string Name { get; set; } 
        public long ObjectId { get; set; } 
        public long ParentId { get; set; } 
        public Vector3? Position { get; set; }
        public Quaternion? Rotation { get; set; } 
        public Vector3? Scale { get; set; } 
        public Dictionary<string, object> Properties { get; set; } 
    }
}
