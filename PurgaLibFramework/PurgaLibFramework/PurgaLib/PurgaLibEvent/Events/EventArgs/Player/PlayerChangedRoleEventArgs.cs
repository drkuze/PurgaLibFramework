using PlayerRoles;

namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerChangedRoleEventArgs : System.EventArgs
    {
        public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }
        public ReferenceHub ReferenceHub => Player?.ReferenceHub;
        public PlayerRoleBase NewRole { get; }
        public RoleTypeId OldRole { get; }
        public RoleChangeReason ChangeReason { get; }
        public RoleSpawnFlags SpawnFlags { get; }
        public bool IsAllowed { get; set; } = true;

        public PlayerChangedRoleEventArgs(
            PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player,
            RoleTypeId oldRole,
            PlayerRoleBase newRole,
            RoleChangeReason changeReason,
            RoleSpawnFlags spawnFlags)
        {
            Player = player;
            OldRole = oldRole;
            NewRole = newRole;
            ChangeReason = changeReason;
            SpawnFlags = spawnFlags;
        }
    }
}