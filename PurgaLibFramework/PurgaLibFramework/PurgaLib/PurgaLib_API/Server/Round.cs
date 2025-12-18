namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLib_API.Server
{
    public static class Round
    {
        public static void Restart()
        {
            LabApi.Features.Wrappers.Round.Restart();
        }
        public static void Start()
        {
            LabApi.Features.Wrappers.Round.Start();
        }
        public static void Stop()
        {
            LabApi.Features.Wrappers.Round.End();
        }
    }
}