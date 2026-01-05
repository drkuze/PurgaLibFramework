using PlayerRoles;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player
{
    public static class State
    {
        public static string Name(LabApi.Features.Wrappers.Player player)
        {
            return player?.Nickname;
        }

        public static int Id(LabApi.Features.Wrappers.Player player)
        {
            return player?.PlayerId ?? -1;
        }

        public static RoleTypeId Role(LabApi.Features.Wrappers.Player player)
        {
            return player?.Role ?? RoleTypeId.None;
        }

        public static float Health(LabApi.Features.Wrappers.Player player)
        {
            return player?.Health ?? 0f;
        }

        public static (float x, float y, float z) Position(LabApi.Features.Wrappers.Player player)
        {
            if (player == null)
                return (0f, 0f, 0f);

            var pos = player.Position;
            return (pos.x, pos.y, pos.z);
        }
    }
}