using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scp914;

namespace PurgaLib.API.Features
{
    public static class Scp914
    {
        public static Scp914Controller Base => Scp914Controller.Singleton;

        public static GameObject GameObject => Base.gameObject;

        public static Transform Transform => Base.transform;

        public static Scp914KnobSetting KnobSetting
        {
            get => Base.Network_knobSetting;
            set => Base.Network_knobSetting = value;
        }

        public static Scp914Mode Mode
        {
            get => Base.ConfigMode.Value;
            set => Base.ConfigMode.Value = value;
        }

        public static bool IsWorking => Base.IsUpgrading;

        public static Vector3 IntakePosition => Base.IntakeChamber.position;

        public static Vector3 OutputPosition => Base.OutputChamber.position;

        public static Vector3 MovingVector => OutputPosition - IntakePosition;

        public static Collider[] InsideIntake =>
            Physics.OverlapBox(Base.IntakeChamber.position, Base.IntakeChamberSize);

        public static IEnumerable<Player> PlayersInside =>
            InsideIntake
                .Select(c => Player.Get(c.transform.root.gameObject))
                .Where(p => p != null);

        public static IEnumerable<Pickup> PickupsInside =>
            InsideIntake
                .Select(c => Pickup.Get(c.transform.root.gameObject))
                .Where(p => p != null && !p.IsLocked);

        public static IEnumerable<GameObject> ObjectsInside =>
            InsideIntake
                .Select(c => c.transform.root.gameObject)
                .Distinct();

        public static IReadOnlyCollection<Door> Doors =>
            Base.Doors
                .Select(Door.Get)
                .Where(d => d != null)
                .ToList();

        public static void PlaySound(Scp914InteractCode sound) =>
            Base.RpcPlaySound((byte)sound);

        public static void Start(Player player = null,
            Scp914InteractCode code = Scp914InteractCode.Activate)
        {
            var hub = (player ?? Server.Features.Host).ReferenceHub;
            Base.ServerInteract(hub, (byte)code);
        }
    }
}
