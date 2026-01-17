using HarmonyLib;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;

public static class PatchALL
{
    private static Harmony _harmony;
    public static void PatchAll()
    {
        _harmony = new Harmony("PurgaLibFramework");
        _harmony.PatchAll();
    }
    public static void UnPatchAll()
    {
        _harmony.UnpatchAll();
    }
}