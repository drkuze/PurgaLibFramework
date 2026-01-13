using PlayerRoles;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerChangedRoleEventArgs : System.EventArgs
    {
        public PurgaLibAPI.Features.Player Player { get; }
        public ReferenceHub ReferenceHub => Player?.ReferenceHub;
        public PlayerRoleBase NewRole { get; }
        public RoleTypeId OldRole { get; }
        public RoleChangeReason ChangeReason { get; }
        public RoleSpawnFlags SpawnFlags { get; }
        public bool IsAllowed { get; set; } = true;

        public PlayerChangedRoleEventArgs(
            PurgaLibAPI.Features.Player player,
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