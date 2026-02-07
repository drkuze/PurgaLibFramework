using UnityEngine;

namespace PurgaLib.API.Features.Schematics;

public class Vector3Json
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

public class RotationJson
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public Quaternion ToQuaternion()
    {
        return Quaternion.Euler(x, y, z);
    }
}

public class ScaleJson
{
    public float x { get; set; } = 1;
    public float y { get; set; } = 1;
    public float z { get; set; } = 1;

    public Vector3 ToVector3() => new Vector3(x, y, z);
}