using MapGeneration;
using System.Collections.Generic;
using System.Linq;
using PurgaLib.API.Enums;
using PurgaLib.API.Extensions;
using UnityEngine;

namespace PurgaLib.API.Features
{
    public class Room
    {
        private static readonly Dictionary<RoomIdentifier, Room> Dictionary = new();

        public RoomIdentifier Base { get; }

        public Transform Transform => Base.transform;

        public GameObject GameObject => Base.gameObject;

        public Vector3 Position => Transform.position;
        
        public RoomType Type { get; set; }

        public ZoneType Zone => ZoneTypeExtensions.FromFacility(Base.Zone);

        private Room(RoomIdentifier room)
        {
            Base = room;
            Dictionary[room] = this;
        }

        public static IReadOnlyCollection<Room> List
        {
            get
            {
                if (Dictionary.Count == 0)
                {
                    foreach (var r in RoomIdentifier.AllRoomIdentifiers)
                        _ = Get(r);
                }

                return Dictionary.Values;
            }
        }

        public static void Refresh()
        {
            Dictionary.Clear();
        }

        public static Room Get(RoomIdentifier identifier)
        {
            if (identifier == null) return null;

            if (Dictionary.TryGetValue(identifier, out var room))
                return room;

            return new Room(identifier);
        }

        public static Room Get(GameObject obj)
        {
            if (obj == null) return null;

            var id = obj.GetComponentInParent<RoomIdentifier>();
            return Get(id);
        }

        public static Room Get(Vector3 position)
        {
            return List
                .OrderBy(r => Vector3.Distance(r.Position, position))
                .FirstOrDefault();
        }

        public static Room Random()
        {
            var list = List.ToList();
            if (list.Count == 0) return null;

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static IEnumerable<Room> InZone(FacilityZone zone) =>
            List.Where(r => r.Zone == (ZoneType)zone);

        public float Distance(Vector3 point) =>
            Vector3.Distance(Position, point);

        public bool Contains(Vector3 point, float radius = 10f) =>
            Distance(point) <= radius;

        public Vector3 GetRandomPoint(float radius = 3f)
        {
            return Position + new Vector3(
                UnityEngine.Random.Range(-radius, radius),
                1f,
                UnityEngine.Random.Range(-radius, radius)
            );
        }
        public static Room FindParentRoom(GameObject obj)
        {
            if (obj == null) return null;
            return List.FirstOrDefault(r => r.Contains(obj.transform.position));
        }

        public override string ToString() =>
            $"Room({Zone} @ {Position})";
    }
}
