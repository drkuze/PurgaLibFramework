using System.Collections.Generic;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Core;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features
{
    public sealed class AutoWhitelist : PActor
    {
        private readonly HashSet<string> _whitelist = new();
        private bool _isActive;
        private const string KickReason = "Server is in whitelist mode";

        public bool IsActive => _isActive;

        public void Enable()
        {
            if (_isActive) return;
            _isActive = true;
            Log.Info("Server whitelist mode enabled");
            KickNonWhitelistedPlayers();
        }

        public void Disable()
        {
            if (!_isActive) return;
            _isActive = false;
            Log.Info("Server whitelist mode disabled");
        }

        private void KickNonWhitelistedPlayers()
        {
            foreach (var player in Player.List)
            {
                if (!_whitelist.Contains(player.UserId))
                {
                    player.Kick(KickReason);
                    Log.Info($"Kicked {player.Nickname} ({player.UserId}) because server is whitelisted");
                }
            }
        }

        public void AddToWhitelist(Player player)
        {
            if (_whitelist.Add(player.UserId))
                Log.Info($"Added {player.Nickname} ({player.UserId}) to whitelist");
        }

        public void RemoveFromWhitelist(Player player)
        {
            if (_whitelist.Remove(player.UserId))
                Log.Info($"Removed {player.Nickname} ({player.UserId}) from whitelist");
        }

        public bool IsWhitelisted(Player player) => _whitelist.Contains(player.UserId);

        protected override void Tick()
        {
            if (!_isActive) return;

            foreach (var player in Player.List)
            {
                if (!_whitelist.Contains(player.UserId))
                {
                    player.Kick(KickReason);
                    Log.Info($"Kicked {player.Nickname} ({player.UserId}) during Tick check");
                }
            }
        }

        public override bool IsAlive => true;
        public override UnityEngine.Transform Transform => null;
    }

    public static class WhitelistFeature
    {
        private static readonly AutoWhitelist Core = StaticActor.Get<AutoWhitelist>();

        public static void Enable() => Core.Enable();
        public static void Disable() => Core.Disable();
        public static void AddUser(Player player) => Core.AddToWhitelist(player);
        public static void RemoveUser(Player player) => Core.RemoveFromWhitelist(player);
        public static bool IsUserWhitelisted(Player player) => Core.IsWhitelisted(player);
    }
}
