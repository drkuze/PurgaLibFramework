using System.Collections.Generic;
using System.Linq;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using Mirror;
using UnityEngine;

namespace PurgaLib.API.Features
{
    public class Pickup
    {
        internal static Dictionary<ItemPickupBase, Pickup> Cache { get; } = new();

        public static IReadOnlyCollection<Pickup> List => Cache.Values;

        public static Pickup Get(ItemPickupBase pickupBase)
        {
            if (pickupBase == null)
                return null;

            if (Cache.TryGetValue(pickupBase, out var pickup))
                return pickup;

            return new Pickup(pickupBase);
        }

        public static Pickup Get(GameObject gameObject)
        {
            if (!gameObject || !gameObject.TryGetComponent(out ItemPickupBase ipb))
                return null;

            return Get(ipb);
        }

        public static Pickup Get(ushort serial) =>
            List.FirstOrDefault(x => x.Serial == serial);

        public static IEnumerable<Pickup> Get(ItemType type) =>
            List.Where(x => x.Type == type);

        public static Pickup Create(ItemType type)
        {
            if (!InventoryItemLoader.AvailableItems.TryGetValue(type, out var itemBase))
                return null;

            var pickupBase = Object.Instantiate(itemBase.PickupDropModel);

            pickupBase.Info = new PickupSyncInfo
            {
                ItemId = type,
                Serial = ItemSerialGenerator.GenerateNext(),
                WeightKg = itemBase.Weight,
            };

            return Get(pickupBase);
        }

        public static Pickup CreateAndSpawn(ItemType type, Vector3 position, Quaternion? rotation = null, Player previousOwner = null)
        {
            var pickup = Create(type);
            pickup?.Spawn(position, rotation, previousOwner);
            return pickup;
        }

        protected Pickup(ItemPickupBase pickupBase)
        {
            Base = pickupBase;

            if (pickupBase.Info.ItemId != ItemType.None)
                Cache[pickupBase] = this;
        }

        public ItemPickupBase Base { get; protected set; }

        public GameObject GameObject => Base.gameObject;

        public Transform Transform => Base.transform;

        public Rigidbody Rigidbody => Base.PhysicsModule?.Pickup.gameObject.AddComponent<Rigidbody>();

        public ushort Serial
        {
            get => Base.Info.Serial;
            set
            {
                var info = Base.Info;
                info.Serial = value;
                Base.Info = info;
            }
        }

        public ItemType Type => Base.Info.ItemId;

        public float Weight
        {
            get => Base.Info.WeightKg;
            set
            {
                var info = Base.Info;
                info.WeightKg = value;
                Base.Info = info;
            }
        }

        public bool IsLocked
        {
            get => Base.Info.Locked;
            set
            {
                var info = Base.Info;
                info.Locked = value;
                Base.Info = info;
            }
        }

        public bool InUse
        {
            get => Base.Info.InUse;
            set
            {
                var info = Base.Info;
                info.InUse = value;
                Base.Info = info;
            }
        }

        public Vector3 Position
        {
            get => Base.Position;
            set => Base.Position = value;
        }

        public Quaternion Rotation
        {
            get => Base.Rotation;
            set => Base.Rotation = value;
        }

        public bool IsSpawned => NetworkServer.spawned.ContainsValue(Base.netIdentity);

        public Player PreviousOwner
        {
            get => Player.Get(Base.PreviousOwner.Hub);
            set => Base.PreviousOwner = value == null ? Player.Host.Footprint : value.Footprint;
        }

        public void Spawn()
        {
            if (!GameObject.activeSelf)
                GameObject.SetActive(true);

            if (!IsSpawned)
                NetworkServer.Spawn(GameObject);
        }

        public Pickup Spawn(Vector3 position, Quaternion? rotation = null, Player previousOwner = null)
        {
            Position = position;
            Rotation = rotation ?? Quaternion.identity;
            PreviousOwner = previousOwner;
            Spawn();
            return this;
        }

        public void UnSpawn()
        {
            if (IsSpawned)
                NetworkServer.UnSpawn(GameObject);
        }

        public void Destroy()
        {
            Cache.Remove(Base);
            Base.DestroySelf();
        }

        public override string ToString() =>
            $"Pickup(Type={Type}, Serial={Serial}, Pos={Position})";
    }
}
