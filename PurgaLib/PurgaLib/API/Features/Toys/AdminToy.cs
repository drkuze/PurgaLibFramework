using System.Collections.Generic;
using AdminToys;
using Footprinting;
using Mirror;
using UnityEngine;

namespace PurgaLib.API.Features.Toys
{
    public abstract class AdminToy
    {
        internal static readonly Dictionary<AdminToyBase, AdminToy> Cache = new();

        protected AdminToy(AdminToyBase baseToy)
        {
            Base = baseToy;
            Cache[baseToy] = this;
        }
        
        public static IReadOnlyCollection<AdminToy> List => Cache.Values;
        
        public AdminToyBase Base { get; }

        public GameObject GameObject => Base.gameObject;
        public Transform Transform => Base.transform;
        

        public Vector3 Position
        {
            get => Base.transform.position;
            set
            {
                Base.transform.position = value;
                Base.NetworkPosition = value;
            }
        }
        

        public Quaternion Rotation
        {
            get => Base.transform.rotation;
            set
            {
                Base.transform.rotation = value;
                Base.NetworkRotation = value;
            }
        }

        public Vector3 Scale
        {
            get => Base.transform.localScale;
            set
            {
                Base.transform.localScale = value;
                Base.NetworkScale = value;
            }
        }
        

        public Footprint Footprint
        {
            get => Base.SpawnerFootprint;
            set => Base.SpawnerFootprint = value;
        }
        

        public bool IsStatic
        {
            get => Base.IsStatic;
            set => Base.NetworkIsStatic = value;
        }

        public byte MovementSmoothing
        {
            get => Base.MovementSmoothing;
            set => Base.NetworkMovementSmoothing = value;
        }
        

        public void Spawn()
        {
            NetworkServer.Spawn(GameObject);
        }

        public void UnSpawn()
        {
            NetworkServer.UnSpawn(GameObject);
        }

        public void Destroy()
        {
            Cache.Remove(Base);
            NetworkServer.Destroy(GameObject);
        }
        

        public static AdminToy Get(AdminToyBase baseToy)
        {
            if (baseToy == null)
                return null;

            if (Cache.TryGetValue(baseToy, out var toy))
                return toy;

            return baseToy switch
            {
                PrimitiveObjectToy p => new PrimitiveToy(p),
                LightSourceToy l => new LightToy(l),
                TextToy t => new TextToyWrapper(t),
                _ => new GenericToy(baseToy),
            };
        }
    }
}
