using PlayerRoles;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLib_API.Player
{
    public static class Actions
    {
        public static void Kill(LabApi.Features.Wrappers.Player player)
        {
            player.Kill();
        }

        public static void Heal(LabApi.Features.Wrappers.Player player, int health)
        {
            player.Heal(health);
        }

        public static void Teleport(LabApi.Features.Wrappers.Player player, float x, float y, float z)
        {
            player.Position.Set(x, y, z);
        }

        public static void Give(LabApi.Features.Wrappers.Player player, ItemType item)
        {
            player.AddItem(item);
        }

        public static void ChangeRole(LabApi.Features.Wrappers.Player player, RoleTypeId newrole)
        {
            player.SetRole(newrole);
        }
    }
}