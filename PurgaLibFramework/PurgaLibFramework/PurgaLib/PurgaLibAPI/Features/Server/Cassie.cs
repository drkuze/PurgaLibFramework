namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;

public static class Cassie
{
    public static void Message(string message)
    {
        LabApi.Features.Wrappers.Cassie.Message(
            message
            );
    }
    public static void Clear()
    {
        LabApi.Features.Wrappers.Cassie.Clear();
    }
}