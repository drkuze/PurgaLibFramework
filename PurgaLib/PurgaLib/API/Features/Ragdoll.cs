using System.Collections.Generic;
using System.Linq;
using Mirror;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using PurgaLib.API.Core.Interfaces;
using PurgaLib.API.Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PurgaLib.API.Features
{
    public class Ragdoll : IWorldObject
    {
        internal static readonly Dictionary<BasicRagdoll, Ragdoll> BasicRagdollToWrapper = new(250);

        internal Ragdoll(BasicRagdoll baseRagdoll)
        {
            Base = baseRagdoll;
            BasicRagdollToWrapper.Add(baseRagdoll, this);
        }

        public static IReadOnlyCollection<Ragdoll> List => BasicRagdollToWrapper.Values;

        public BasicRagdoll Base { get; }

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

        public Vector3 RagdollScale
        {
            get => Base.transform.localScale;
            set
            {
                if (!IsSpawned) { Base.transform.localScale = value; return; }
                UnSpawn(); Base.transform.localScale = value; Spawn();
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
                Base.NetworkInfo.StartRelativePosition,
                Base.NetworkInfo.StartRelativeRotation,
                Base.NetworkInfo.Scale,
                value,
                Base.NetworkInfo.CreationTime
            );
        }

        public RoleTypeId Role => Base.NetworkInfo.RoleType;

        public float ExistenceTime => Base.NetworkInfo.ExistenceTime;

        public bool IsFrozen => Base.Frozen;

        public Room Room => Room.FindParentRoom(GameObject);

        public ZoneType Zone => Room?.Zone ?? ZoneType.Unspecified;

        public bool IsSpawned => NetworkServer.spawned.ContainsValue(Base.netIdentity);

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

        public static Ragdoll Get(BasicRagdoll baseRagdoll)
        {
            if (baseRagdoll == null) return null;

            if (BasicRagdollToWrapper.TryGetValue(baseRagdoll, out var wrapper))
                return wrapper;

            return new Ragdoll(baseRagdoll);
        }

        public static IEnumerable<Ragdoll> Get(Player player) => List.Where(r => r.Owner == player);

        public override string ToString() => $"{Owner} ({Name}) *{Role}* |{ExistenceTime}| in {Room}";
    }
}
