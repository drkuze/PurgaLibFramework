using System.Collections.Generic;
using System.Linq;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player
{
    public static class PlayerManager
    {
        public static IReadOnlyCollection<Player> All =>
            LabApi.Features.Wrappers.Player.List
                .Select(Player.Get)
                .Where(p => p != null)
                .ToList();

        public static int Count =>
            LabApi.Features.Wrappers.Player.List.Count;

        public static Player Get(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var lab = LabApi.Features.Wrappers.Player.List
                .FirstOrDefault(p => p.UserId == userId);

            return lab == null ? null : Player.Get(lab);
        }
    }
}