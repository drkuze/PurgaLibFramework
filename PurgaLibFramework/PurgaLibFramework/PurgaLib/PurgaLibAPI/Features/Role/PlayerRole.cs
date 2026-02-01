using PlayerRoles;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Enums;
using UnityEngine;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Role
{
    public class PlayerRole
    {
        internal PlayerRole(Player owner, PlayerRoleBase baseRole)
        {
            Owner = owner;
            Base = baseRole;
        }
        
        public Player Owner { get; }
        
        public PlayerRoleBase Base { get; internal set; }
        
        public RoleTypeId Type => Base.RoleTypeId;

        public Team Team => Base.Team;

        public Color Color => Base.RoleColor;

        public string Name => Base.RoleName;

        public bool IsDead => Team == Team.Dead;

        public bool IsAlive => !IsDead;
        
        public bool IsValid =>
            Owner != null &&
            Owner.IsConnected &&
            Owner.ReferenceHub.roleManager.CurrentRole == Base;
        

        public void Set(RoleTypeId newRole,
            SpawnReason reason = SpawnReason.ForceClass,
            RoleSpawnFlags flags = RoleSpawnFlags.All)
        {
            if (!Owner.IsConnected)
                return;

            Owner.ReferenceHub.roleManager.ServerSetRole(
                newRole,
                (RoleChangeReason)reason,
                flags);
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerRole other && Owner == other.Owner;
        }

        public override int GetHashCode()
        {
            return Owner?.GetHashCode() ?? 0;
        }

        public static bool operator ==(PlayerRole left, PlayerRole right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Owner == right.Owner;
        }

        public static bool operator !=(PlayerRole left, PlayerRole right)
            => !(left == right);

        public static bool operator ==(PlayerRole role, RoleTypeId type)
            => role?.Type == type;

        public static bool operator !=(PlayerRole role, RoleTypeId type)
            => role?.Type != type;
    }
}
