using PlayerRoles;

namespace PurgaLib.API.Extensions
{
    public static class RoleTypeExtensions
    {
        public static bool IsSCP(this RoleTypeId role)
        {
            return role == RoleTypeId.Scp049 ||
                   role == RoleTypeId.Scp096 ||
                   role == RoleTypeId.Scp106 ||
                   role == RoleTypeId.Scp173 ||
                   role == RoleTypeId.Scp079 ||
                   role == RoleTypeId.Scp939;
        }

        public static bool IsHuman(this RoleTypeId role)
        {
            return !role.IsSCP() && role != RoleTypeId.Spectator;
        }
        public static Team FactionToTeam(Faction faction)
        {
            return faction switch
            {
                Faction.FoundationStaff => Team.FoundationForces,
                Faction.FoundationEnemy => Team.ChaosInsurgency,
                Faction.SCP => Team.SCPs, 
                Faction.Unclassified => Team.Dead,
                _ => Team.OtherAlive
            };
        }
        
    }
}
