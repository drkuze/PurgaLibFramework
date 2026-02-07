using UnityEngine;

namespace PurgaLib.API.Core.Interfaces;

public interface ISchematic
{
    string Path { get; } 
    Vector3 Position { get; }
    Quaternion Rotation { get; }
    
    //If spawn on round start. 
    bool SpawnOnRoundStart { get; }
    //If you want a void that when is called it spawns the schematic. <- Btw first comments in code (aura)
    void SpawnSchematic();
}