using PlayerRoles;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerChangingRoleEventArgs : System.EventArgs
    {
        public LabApi.Features.Wrappers.Player Player { get; }
        public RoleTypeId OldRole { get; }
        public PlayerRoleBase NewRole { get; set; }
        public bool IsAllowed { get; set; } = true;

        public PlayerChangingRoleEventArgs(LabApi.Features.Wrappers.Player player, RoleTypeId oldRole, PlayerRoleBase newRole)
        {
            Player = player;
            OldRole = oldRole;
            NewRole = newRole;
        }
    }
}