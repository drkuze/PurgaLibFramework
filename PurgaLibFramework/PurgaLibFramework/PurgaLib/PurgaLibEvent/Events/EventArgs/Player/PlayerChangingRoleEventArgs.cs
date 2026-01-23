using PlayerRoles;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player;

public class PlayerChangingRoleEventArgs : IEventArgs
{
    public PurgaLibAPI.Features.Player Player { get; }
    public RoleTypeId OldRole { get; }
    public RoleTypeId NewRole { get; }

    public PlayerChangingRoleEventArgs(PurgaLibAPI.Features.Player player, RoleTypeId oldRole, RoleTypeId newRole)
    {
        Player = player;
        OldRole = oldRole;
        NewRole = newRole;
    }

    public bool IsAllowed { get; set; }
}