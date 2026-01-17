using System.Collections.Generic;
using Interactables.Interobjects;
using UnityEngine;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features
{
    public sealed class Elevator
    {
        private readonly ElevatorDoor _door;
        private static readonly Dictionary<ElevatorDoor, Elevator> Cache = new();

        #region State

        public Elevator(ElevatorDoor door)
        {
            _door = door;
        }
        
        public ElevatorDoor Base => _door;
        public ElevatorGroup ElevatorType => _door.Chamber.AssignedGroup;
        public string Name => _door.gameObject.name;
        public Vector3 Position => _door.transform.position;
        public bool IsOpen => _door.NetworkTargetState;
        public bool IsClosed => !_door.NetworkTargetState;
        #endregion
        
        #region Actions

        public void Open()
        {
            _door.NetworkTargetState = true;
        }

        public void Close()
        {
            _door.NetworkTargetState = false;
        }

        #endregion

        #region Static access

        public static Elevator Get(ElevatorDoor door)
        {
            if (door == null) return null;

            if (!Cache.TryGetValue(door, out var wrapper))
            {
                wrapper = new Elevator(door);
                Cache.Add(door, wrapper);
            }

            return wrapper;
        }

        #endregion

        public override string ToString()
            => $"Elevator(Type={ElevatorType}, Open={IsOpen})";
    }
}
