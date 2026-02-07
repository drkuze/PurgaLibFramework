using PurgaLib.API.Features.Schematics;
using UnityEngine;

namespace PurgaLib.API.Core.Interfaces;

public interface ISchematicBlockFactory
{
    bool CanHandle(SchematicBlock block);
    GameObject Spawn(SchematicBlock block);
}
