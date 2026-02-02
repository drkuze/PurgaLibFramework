namespace PurgaLib.API.Features.Server
{
    public static class Server
    {
        public static int PlayerCount => Player.List.Count;

        public static void Restart() => Round.Restart();

        public static void Stop() => Round.End();

        public static string ExecuteCommand(string command, CommandSender sender = null) => GameCore.Console.singleton.TypeCommand(command, sender);

        public static void Broadcast(string message, float duration = 5f)
        {
            foreach (var hub in ReferenceHub.AllHubs)
            {
                if (hub == null || hub.playerStats == null)
                    continue;
                
                hub.BroadcastMessage(message, duration);
            }
        }
    }
}
