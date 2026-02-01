using PlayerRoles;
using PurgaLib.API.Features;
using PurgaLib.Events.EventSystem.Interfaces;

public class PlayerChangedRoleEventArgs : IEventArgs
{
    public Player Player { get; }
    public RoleTypeId OldRole { get; }
    public RoleTypeId NewRole { get; }

    public PlayerChangedRoleEventArgs(
        Player player,
        RoleTypeId oldRole,
        RoleTypeId newRole
        )
    {
        Player = player;
        OldRole = oldRole;
        NewRole = newRole;
    }

    public bool IsAllowed { get; set; }
}
