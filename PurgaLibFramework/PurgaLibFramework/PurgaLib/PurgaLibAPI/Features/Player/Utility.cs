namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player
{
    public static class Utility
    {
        public static void SendBroadcast(LabApi.Features.Wrappers.Player player, string message, ushort duration)
        {
            if (player == null || string.IsNullOrEmpty(message))
                return;

            player.SendBroadcast(message, duration);
        }

        public static void SendHint(LabApi.Features.Wrappers.Player player, string message, float duration)
        {
            if (player == null || string.IsNullOrEmpty(message))
                return;

            player.SendHint(message, duration);
        }
    }
}