using System.Collections.Generic;
using System.Linq;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp3114;
using PurgaLib.API.Core.Interfaces;
using PurgaLib.API.Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PurgaLib.API.Features
{
    public class RagdollScp3114 : IWorldObject
    {
        internal static readonly Dictionary<Scp3114Ragdoll, RagdollScp3114> Wrappers = new(50);

        internal RagdollScp3114(Scp3114Ragdoll baseRagdoll)
        {
            Base = baseRagdoll;
            Wrappers[baseRagdoll] = this;
        }

        public static IReadOnlyCollection<RagdollScp3114> List => Wrappers.Values;

        public Scp3114Ragdoll Base { get; }

        public GameObject GameObject => Base.gameObject;

        public Transform Transform => Base.transform;

        public Player Owner => Player.Get(Base.NetworkInfo.OwnerHub);

        public Vector3 Position
        {
            get => Base.transform.position;
            set
            {
                if (!IsSpawned) { Base.transform.position = value; return; }
                UnSpawn(); Base.transform.position = value; Spawn();
            }
        }

        public Quaternion Rotation
        {
            get => Base.transform.rotation;
            set
            {
                if (!IsSpawned) { Base.transform.rotation = value; return; }
                UnSpawn(); Base.transform.rotation = value; Spawn();
            }
        }

        public string Name => Base.name;

        public string Nickname
        {
            get => Base.NetworkInfo.Nickname;
            set => Base.NetworkInfo = new(
                Base.NetworkInfo.OwnerHub,
                Base.NetworkInfo.Handler,
                Base.NetworkInfo.RoleType,
                Base.NetworkInfo.StartPosition,
                Base.NetworkInfo.StartRotation,
                Base.NetworkInfo.Scale,
                value,
                Base.NetworkInfo.CreationTime
            );
        }

        public RoleTypeId Role => Base.NetworkInfo.RoleType;

        public bool IsSpawned => NetworkServer.spawned.ContainsValue(Base.netIdentity);

        public Room Room => Room.FindParentRoom(GameObject);

        public ZoneType Zone => Room?.Zone ?? ZoneType.Unspecified;

        public void Destroy() => Object.Destroy(GameObject);

        public void Destroy(float delay) => Object.Destroy(GameObject, delay);

        public void Spawn() => NetworkServer.Spawn(GameObject);

        public void Spawn(GameObject owner) => NetworkServer.Spawn(GameObject, owner);

        public void Spawn(NetworkConnection ownerConnection, uint? assetId = null)
        {
            if (assetId.HasValue) NetworkServer.Spawn(GameObject, assetId.Value, ownerConnection);
            else NetworkServer.Spawn(GameObject, ownerConnection);
        }

        public void UnSpawn() => NetworkServer.UnSpawn(GameObject);

        public static RagdollScp3114 Get(Scp3114Ragdoll baseRagdoll)
        {
            if (baseRagdoll == null) return null;
            return Wrappers.TryGetValue(baseRagdoll, out var wrapper) ? wrapper : new RagdollScp3114(baseRagdoll);
        }

        public static IEnumerable<RagdollScp3114> Get(Player player) =>
            List.Where(r => r.Owner == player);

        public override string ToString() =>
            $"{Owner} ({Name}) *{Role}* | in {Room}";
    }
}
