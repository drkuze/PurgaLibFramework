using PlayerRoles;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerChangingRoleEventArgs : System.EventArgs
    {
        public PurgaLibAPI.Features.Player Player { get; }
        public PlayerRoleBase OldRole { get; }
        public RoleTypeId NewRole { get; set; }
        public bool IsAllowed { get; set; } = true;

        public PlayerChangingRoleEventArgs(PurgaLibAPI.Features.Player player, PlayerRoleBase oldRole, RoleTypeId newRole)
        {
            Player = player;
            OldRole = oldRole;
            NewRole = newRole;
        }
    }
}