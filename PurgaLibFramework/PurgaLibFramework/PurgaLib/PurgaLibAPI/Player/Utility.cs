namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLib_API.Player
{
    public static class Utility
    {
        public static void SendBroadCast(LabApi.Features.Wrappers.Player player, string message, ushort duration)
        {
            player.SendBroadcast(message, duration);
        }

        public static void SendHint(LabApi.Features.Wrappers.Player player, string message, float duration)
        {
            player.SendHint(message, duration);
        }
    }
}