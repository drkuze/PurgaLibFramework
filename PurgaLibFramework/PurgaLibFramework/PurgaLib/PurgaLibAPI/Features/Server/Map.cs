using UnityEngine;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;

public static class Map
{
    public static void TurnOffAllLights(ushort duration)
    {
        LabApi.Features.Wrappers.Map.TurnOffLights(duration);
    }

    public static void ResetAllLights()
    {
        LabApi.Features.Wrappers.Map.ResetColorOfLights();
    }

    public static void SetColorOfAllLights(Color color)
    {
        LabApi.Features.Wrappers.Map.SetColorOfLights(color);
    }

    public static void TurnOnAllLights()
    {
        PluginAPI.Core.Map.TurnOnAllLights();
    }
}