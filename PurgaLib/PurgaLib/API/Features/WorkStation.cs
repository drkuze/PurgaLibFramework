using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using InventorySystem.Items.Firearms.Attachments;
using Mirror;
using UnityEngine;

namespace PurgaLib.API.Features
{
    public class Workstation
    {
        internal static Dictionary<WorkstationController, Workstation> Cache { get; } = new();

        public static IReadOnlyCollection<Workstation> List => Cache.Values;

        public static Workstation Get(WorkstationController controller)
        {
            if (controller == null)
                return null;

            if (Cache.TryGetValue(controller, out var workstation))
                return workstation;

            return new Workstation(controller);
        }

        public static IEnumerable<Workstation> Get(System.Func<Workstation, bool> predicate) =>
            List.Where(predicate);

        protected Workstation(WorkstationController controller)
        {
            Base = controller;
            Cache[controller] = this;
        }

        public WorkstationController Base { get; }

        public GameObject GameObject => Base.gameObject;

        public Transform Transform => Base.transform;

        public Vector3 Position
        {
            get => Transform.position;
            set
            {
                Respawn();
                Transform.position = value;
                Respawn();
            }
        }

        public Quaternion Rotation
        {
            get => Transform.rotation;
            set
            {
                Respawn();
                Transform.rotation = value;
                Respawn();
            }
        }

        public WorkstationController.WorkstationStatus Status
        {
            get => (WorkstationController.WorkstationStatus)Base.Status;
            set => Base.NetworkStatus = (byte)value;
        }

        public Stopwatch Stopwatch => Base.ServerStopwatch;

        public Player KnownUser
        {
            get => Player.Get(Base.KnownUser);
            set => Base.KnownUser = value?.ReferenceHub;
        }

        public bool IsSpawned => NetworkServer.spawned.ContainsValue(Base.netIdentity);

        public bool IsInRange(Player player) =>
            player != null && Base.IsInRange(player.ReferenceHub);

        public void Interact(Player player)
        {
            if (player == null)
                return;

            Base.ServerInteract(player.ReferenceHub, Base.ActivateCollider.ColliderId);
        }

        public void Respawn()
        {
            if (IsSpawned)
            {
                NetworkServer.UnSpawn(GameObject);
                NetworkServer.Spawn(GameObject);
            }
        }

        public override string ToString() =>
            $"Workstation(Pos={Position}, Status={Status})";
    }
}
