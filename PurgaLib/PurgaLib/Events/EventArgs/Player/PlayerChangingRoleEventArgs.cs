using PlayerRoles;
using PurgaLib.Events.EventSystem.Interfaces;

namespace PurgaLib.Events.EventArgs.Player;

public class PlayerChangingRoleEventArgs : IEventArgs
{
    public global::PurgaLib.API.Features.Player Player { get; }
    public RoleTypeId OldRole { get; }
    public RoleTypeId NewRole { get; set; }
    public PlayerChangingRoleEventArgs(global::PurgaLib.API.Features.Player player, RoleTypeId oldRole, RoleTypeId newRole)
    {
        Player = player;
        OldRole = oldRole;
        NewRole = newRole;
        IsAllowed = true;
    }

    public bool IsAllowed { get; set; } = true;
}