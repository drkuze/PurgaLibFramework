namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLib_API.Server
{
    public static class Server
    {
        public static void Restart()
        {
            LabApi.Features.Wrappers.Server.Restart();
        }
        public static void Stop()
        {
            LabApi.Features.Wrappers.Server.Shutdown();
        }
        public static void BroadCast(string message, ushort duration)
        {
            LabApi.Features.Wrappers.Server.SendBroadcast(message, duration);
        }
    }
}