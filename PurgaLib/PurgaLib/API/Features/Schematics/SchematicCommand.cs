using System;

namespace PurgaLib.API.Features.Schematics;

using CommandSystem;
using Newtonsoft.Json;
using UnityEngine;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SchematicCommand : ICommand
{
    public string Command => "spawnschematic";
    public string[] Aliases => new[] { "ss" };
    public string Description => "Spawns a schematic.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        try
        {
            string json = string.Join(" ", arguments);
            var schematic = JsonConvert.DeserializeObject<SchematicRoot>(json);

            if (schematic == null)
            {
                response = "Invalid JSON";
                return false;
            }

            SchematicSpawner.Spawn(schematic, Vector3.zero);
            response = "Spawned schematic!";
            return true;
        }
        catch (Exception e)
        {
            response = $"Error: {e.Message}";
            return false;
        }
    }
}
