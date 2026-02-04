using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PurgaLib.API.Features.Server
{
    public static class Features
    {
        private static readonly Dictionary<string, object> _session = new();

        public static IReadOnlyCollection<Player> Players => Player.List;

        public static int PlayerCount => Player.List.Count;

        public static Player Host => Player.Host;

        public static string Name
        {
            get => ServerConsole.ServerName;
            set
            {
                ServerConsole.ServerName = value;
                ServerConsole.Singleton.RefreshServerNameSafe();
            }
        }

        public static string Version => GameCore.Version.VersionString;

        public static ushort Port => ServerStatic.ServerPort;

        public static bool FriendlyFire
        {
            get => ServerConsole.FriendlyFire;
            set => ServerConsole.FriendlyFire = value;
        }

        public static int MaxPlayers
        {
            get => CustomNetworkManager.slots;
            set => CustomNetworkManager.slots = value;
        }

        public static double TPS
        {
            get
            {
                var dt = Time.deltaTime;
                return dt <= 0 ? 60 : Math.Round(1f / dt);
            }
        }

        public static void Restart() =>
            Round.Restart(false, true, ServerStatic.NextRoundAction.Restart);

        public static void RestartFast() =>
            Round.Restart(true, true, ServerStatic.NextRoundAction.Restart);

        public static void Stop() =>
            global::Shutdown.Quit();

        public static string ExecuteCommand(string command, CommandSender sender = null) =>
            GameCore.Console.Singleton.TypeCommand(command, sender);

        public static void Broadcast(string message, float duration = 5f)
        {
            foreach (var hub in ReferenceHub.AllHubs)
            {
                if (hub == null || hub.playerStats == null)
                    continue;

                hub.BroadcastMessage(message, duration);
            }
        }

        public static void Broadcast(Player player, string message, float duration = 5f)
        {
            player?.ReferenceHub?.BroadcastMessage(message, duration);
        }

        public static Player GetPlayer(int id) =>
            Player.List.FirstOrDefault(p => p.PlayerId == id);

        public static void SetSession(string key, object value) =>
            _session[key] = value;

        public static bool TryGetSession<T>(string key, out T value)
        {
            if (_session.TryGetValue(key, out var obj) && obj is T cast)
            {
                value = cast;
                return true;
            }

            value = default;
            return false;
        }

        public static void RemoveSession(string key) =>
            _session.Remove(key);

        public static void ClearSession() =>
            _session.Clear();
    }
}
