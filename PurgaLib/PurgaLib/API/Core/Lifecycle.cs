namespace PurgaLib.API.Core
{
    public static class Lifecycle
    {
        public static bool IsInitialized { get; private set; }

        public static void Initialize()
        {
            if (IsInitialized)
                return;

            IsInitialized = true;
        }

        public static void Shutdown()
        {
            IsInitialized = false;
        }
    }
}
