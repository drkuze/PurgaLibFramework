using System.Collections.Generic;

namespace PurgaLib.API.Features.Schematics;

public class SchematicRoot : IJsonSerializable
{
    public long RootObjectId { get; set; }
    public List<SchematicBlock> Blocks { get; set; }
}
