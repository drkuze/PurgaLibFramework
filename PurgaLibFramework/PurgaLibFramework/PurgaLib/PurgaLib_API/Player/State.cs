namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLib_API.Player
{
    public static class State
    {
        public static void Name(LabApi.Features.Wrappers.Player player)
        {
            player.Nickname.ToString();
        }

        public static void Id(LabApi.Features.Wrappers.Player player)
        {
            player.PlayerId.ToString();
        }

        public static void Role(LabApi.Features.Wrappers.Player player)
        {
            player.Role.ToString();
        }

        public static void Health(LabApi.Features.Wrappers.Player player)
        {
            player.Health.ToString();
        }

        public static void Position(LabApi.Features.Wrappers.Player player)
        {
            player.Position.ToString();
        }
    }
}