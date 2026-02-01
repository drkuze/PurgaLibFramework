using UnityEngine;

namespace PurgaLib.API.Core.Interfaces
{
    public interface IWorldObject
    {
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
    }
}
