using System;
using System.Collections.Generic;
using LabApi.Features.Wrappers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCredit
{
    public static class CreditTagsHandler
    {
        private static readonly HashSet<string> Contributors = new()
        {
            "76561199548842223@steam"
        };
        
        public static bool IsContributor(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            return Contributors.Contains(player.UserId);
        }
        
        public static void ApplyContributorTag(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            if (!IsContributor(player))
                return;

            Log.Info($"[PurgaLib] {player.Nickname} is a PurgaLib Contributor!");
            SetPlayerRank(player, "PurgaLibContributor");
        }
        
        private static void SetPlayerRank(Player player, string rank)
        {
            if (player == null || string.IsNullOrEmpty(rank))
                return;

            var roles = player.ReferenceHub.serverRoles;
            roles.GlobalHidden = true;
            roles.GlobalBadge = rank;       
        }
    }
}