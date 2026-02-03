using System;
using System.Collections.Generic;
using System.Linq;
using MapGeneration;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using UnityEngine;
using CameraType = PurgaLib.API.Enums.CameraType;

namespace PurgaLib.API.Features
{
    public class Camera
    {
        internal static readonly Dictionary<Scp079Camera, Camera> Camera079ToCamera;

        private static readonly Dictionary<string, CameraType> NameToCameraType = new()
        {
            ["CHKPT EZ HALL"] = CameraType.EzChkptHall,
            ["EZ CROSSING"] = CameraType.EzCrossing,
            ["EZ CURVE"] = CameraType.EzCurve,
            ["EZ HALLWAY"] = CameraType.EzHallway,
            ["EZ THREE-WAY"] = CameraType.EzThreeWay,
            ["GATE A"] = CameraType.EzGateA,
            ["GATE B"] = CameraType.EzGateB,
            ["INTERCOM BOTTOM"] = CameraType.EzIntercomBottom,
            ["INTERCOM HALL"] = CameraType.EzIntercomHall,
            ["INTERCOM PANEL"] = CameraType.EzIntercomPanel,
            ["INTERCOM STAIRS"] = CameraType.EzIntercomStairs,
            ["LARGE OFFICE"] = CameraType.EzLargeOffice,
            ["LOADING DOCK"] = CameraType.EzLoadingDock,
            ["MINOR OFFICE"] = CameraType.EzMinorOffice,
            ["TWO-STORY OFFICE"] = CameraType.EzTwoStoryOffice,
            ["049 OUTSIDE"] = CameraType.Hcz049Outside,
            ["049 CONT CHAMBER"] = CameraType.Hcz049ContChamber,
            ["049/173 TOP"] = CameraType.Hcz049ElevTop,
            ["049 HALLWAY"] = CameraType.Hcz049Hallway,
            ["173 OUTSIDE"] = CameraType.Hcz173Outside,
            ["049/173 BOTTOM"] = CameraType.Hcz049TopFloor,
            ["079 AIRLOCK"] = CameraType.Hcz079Airlock,
            ["079 CONT CHAMBER"] = CameraType.Hcz079ContChamber,
            ["079 HALLWAY"] = CameraType.Hcz079Hallway,
            ["079 KILL SWITCH"] = CameraType.Hcz079KillSwitch,
            ["096 CONT CHAMBER"] = CameraType.Hcz096ContChamber,
            ["106 BRIDGE"] = CameraType.Hcz106Bridge,
            ["106 CATWALK"] = CameraType.Hcz106Catwalk,
            ["106 RECONTAINMENT"] = CameraType.Hcz106Recontainment,
            ["CHKPT (EZ)"] = CameraType.HczChkptEz,
            ["CHKPT (HCZ)"] = CameraType.HczChkptHcz,
            ["HCZ 939"] = CameraType.Hcz939,
            ["HCZ ARMORY"] = CameraType.HczArmory,
            ["HCZ ARMORY INTERIOR"] = CameraType.HczArmoryInterior,
            ["HCZ CROSSING"] = CameraType.HczCrossing,
            ["HCZ ELEV SYS A"] = CameraType.HczElevSysA,
            ["HCZ ELEV SYS B"] = CameraType.HczElevSysB,
            ["HCZ HALLWAY"] = CameraType.HczHallway,
            ["HCZ THREE-WAY"] = CameraType.HczThreeWay,
            ["TESLA GATE"] = CameraType.HczTeslaGate,
            ["TESTROOM BRIDGE"] = CameraType.HczTestroomBridge,
            ["TESTROOM MAIN"] = CameraType.HczTestroomMain,
            ["TESTROOM OFFICE"] = CameraType.HczTestroomOffice,
            ["WARHEAD ARMORY"] = CameraType.HczWarheadArmory,
            ["WARHEAD CONTROL"] = CameraType.HczWarheadControl,
            ["WARHEAD HALLWAY"] = CameraType.HczWarheadHallway,
            ["173 BOTTOM"] = CameraType.Lcz173Bottom,
            ["173 HALL"] = CameraType.Lcz173Hall,
            ["914 AIRLOCK"] = CameraType.Lcz914Airlock,
            ["914 CONT CHAMBER"] = CameraType.Lcz914ContChamber,
            ["AIRLOCK"] = CameraType.LczAirlock,
            ["ARMORY"] = CameraType.LczArmory,
            ["CELLBLOCK BACK"] = CameraType.LczCellblockBack,
            ["CELLBLOCK ENTRY"] = CameraType.LczCellblockEntry,
            ["CHKPT A ENTRY"] = CameraType.LczChkptAEntry,
            ["CHKPT A INNER"] = CameraType.LczChkptAInner,
            ["CHKPT B ENTRY"] = CameraType.LczChkptBEntry,
            ["CHKPT B INNER"] = CameraType.LczChkptBInner,
            ["GLASSROOM"] = CameraType.LczGlassroom,
            ["GLASSROOM ENTRY"] = CameraType.LczGlassroomEntry,
            ["GREENHOUSE"] = CameraType.LczGreenhouse,
            ["LCZ CROSSING"] = CameraType.LczCrossing,
            ["LCZ CURVE"] = CameraType.LczCurve,
            ["LCZ ELEV SYS A"] = CameraType.LczElevSysA,
            ["LCZ ELEV SYS B"] = CameraType.LczElevSysB,
            ["LCZ HALLWAY"] = CameraType.LczHallway,
            ["LCZ THREE-WAY"] = CameraType.LczThreeWay,
            ["PC OFFICE"] = CameraType.LczPcOffice,
            ["RESTROOMS"] = CameraType.LczRestrooms,
            ["TC HALLWAY"] = CameraType.LczTcHallway,
            ["TEST CHAMBER"] = CameraType.LczTestChamber,
            ["EXIT PASSAGE"] = CameraType.ExitPassage,
            ["GATE A SURFACE"] = CameraType.GateASurface,
            ["GATE B SURFACE"] = CameraType.GateBSurface,
            ["MAIN STREET"] = CameraType.MainStreet,
            ["SURFACE AIRLOCK"] = CameraType.SurfaceAirlock,
            ["SURFACE BRIDGE"] = CameraType.SurfaceBridge,
            ["TUNNEL ENTRANCE"] = CameraType.TunnelEntrance,
            ["HCZ CURVE"] = CameraType.HczCurve,
            ["JUNK MAIN"] = CameraType.HczJunkMain,
            ["JUNK HALLWAY"] = CameraType.HczJunkHallway,
            ["CORNER DEEP"] = CameraType.HczCornerDeep,
            ["DSS-08"] = CameraType.HczDSS08,
            ["MICROHID STAIRS"] = CameraType.HczMicroHIDStairs,
            ["PIPES HALLWAY"] = CameraType.HczPipesHallway,
            ["PIPES MAIN"] = CameraType.HczPipesMain,
            ["WARHEAD STARBOARD ELEVATOR"] = CameraType.HczWarheadStarboardElevator,
            ["MICROHID MAIN"] = CameraType.HczMicroHIDMain,
            ["MICROHID LAB"] = CameraType.HczMicroHIDLab,
            ["WARHEAD TOP ELEVATORS"] = CameraType.HczWarheadTopElevators,
            ["WARHEAD CONNECTOR"] = CameraType.HczWarheadConnector,
            ["WARHEAD PORT ELEVATOR"] = CameraType.HczWarheadPortElevator,
            ["HCZ SCP-127 LAB"] = CameraType.HczScp127Lab,
            ["HCZ SCP-127 CONTAINMENT"] = CameraType.HczScp127Containment,
            ["HCZ SERVERS UPPER STORAGE"] = CameraType.HczServersUpperStorage,
            ["HCZ LOWER SERVER STORAGE"] = CameraType.HczLowerServerStorage,
            ["HCZ SERVERS STAIRCASE"] = CameraType.HczServerStaircase,
            ["DSS-12"] = CameraType.HczDss12,
            ["GATE A INTERIOR"] = CameraType.EzGateAInterior,
            ["GATE A ELEVATORS"] = CameraType.EzGateAElevators,
            ["GATE B INTERIOR"] = CameraType.EzGateBInterior,
            ["GATE B SIDE"] = CameraType.EzGateBSide,
            ["GATE A STAIRWELL"] = CameraType.EzGateAStairwell,
            ["GATE A UPPER"] = CameraType.EzGateAUpper,
            ["LOADING BAY"] = CameraType.HczLoadingBay,
            ["HCZ LOADING RAMP"] = CameraType.HczLoadingBayRamp,
            ["STAIRWELL"] = CameraType.HczLoadingBayStairwell,
            ["EZ ARM CAMERA TOY"] = CameraType.EzArmCameraToy,
            ["EZ CAMERA TOY"] = CameraType.EzCameraToy,
            ["HCZ CAMERA TOY"] = CameraType.HczCameraToy,
            ["LCZ CAMERA TOY"] = CameraType.LczCameraToy,
            ["SZ CAMERA TOY"] = CameraType.SzCameraToy,
        };
        private Room room;

        static Camera()
        {
            Camera079ToCamera = new Dictionary<Scp079Camera, Camera>(256);

            foreach (var cam in UnityEngine.Object.FindObjectsOfType<Scp079Camera>())
                _ = Get(cam);
        }

        private Camera(Scp079Camera baseCamera)
        {
            Base = baseCamera;
            Camera079ToCamera[baseCamera] = this;
            Type = ResolveType();
        }

        public static IReadOnlyCollection<Camera> List => Camera079ToCamera.Values;

        public static Camera Random =>
            List.OrderBy(_ => UnityEngine.Random.value).FirstOrDefault();

        public Scp079Camera Base { get; }

        public GameObject GameObject => Base.gameObject;

        public Transform Transform => Base.transform;

        public string Name => Base.Label;

        public ushort Id => Base.SyncId;

        public Room Room => room ??= Room.Get(Base.Room);

        public FacilityZone Zone => (FacilityZone)Room.Zone;

        public CameraType Type { get; }

        public Vector3 Position => Base.Position;

        public Quaternion Rotation
        {
            get => Base.CameraAnchor.rotation;
            set => Base.CameraAnchor.rotation = value;
        }

        public float Zoom => Base.ZoomAxis.CurrentZoom;

        public bool IsBeingUsed
        {
            get => Base.IsActive;
            set => Base.IsActive = value;
        }

        public static Camera Get(Scp079Camera camera)
        {
            if (camera == null)
                return null;

            if (Camera079ToCamera.TryGetValue(camera, out var wrap))
                return wrap;

            return new Camera(camera);
        }

        public static Camera Get(uint id) =>
            List.FirstOrDefault(x => x.Id == id);

        public static Camera Get(string name) =>
            List.FirstOrDefault(x => x.Name == name);

        public static Camera Get(CameraType type) =>
            List.FirstOrDefault(x => x.Type == type);

        public static IEnumerable<Camera> Get(Func<Camera, bool> predicate) =>
            List.Where(predicate);

        public static bool TryGet(uint id, out Camera result) =>
            (result = Get(id)) != null;

        public static bool TryGet(string name, out Camera result) =>
            (result = Get(name)) != null;

        public static bool TryGet(CameraType type, out Camera result) =>
            (result = Get(type)) != null;

        public static bool TryGet(Func<Camera, bool> predicate, out IEnumerable<Camera> result) =>
            (result = Get(predicate)).Any();

        public override string ToString() =>
            $"{Type} | {Name} | {Zone} | {Id}";

        private CameraType ResolveType()
        {
            if (NameToCameraType.TryGetValue(Name, out var type))
                return type;

            return CameraType.Unknown;
        }
    }
}

