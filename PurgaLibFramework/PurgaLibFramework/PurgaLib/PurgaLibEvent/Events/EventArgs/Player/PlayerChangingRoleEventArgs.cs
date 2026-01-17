using PlayerRoles;

namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerChangingRoleEventArgs : System.EventArgs
    {
        public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
        public PlayerRoleBase OldRole { get; }
        public RoleTypeId NewRole { get; set; }
        public bool IsAllowed { get; set; } = true;

        public PlayerChangingRoleEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player, PlayerRoleBase oldRole, RoleTypeId newRole)
        {
            Player = player;
            OldRole = oldRole;
            NewRole = newRole;
        }
    }
}