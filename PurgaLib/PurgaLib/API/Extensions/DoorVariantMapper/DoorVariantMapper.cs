using PurgaLib.API.Enums;
using System;
using System.Collections.Generic;

namespace PurgaLib.API.Extensions.DoorVariantMapper
{
    public static class DoorMapper
    {
        private static readonly Dictionary<string, DoorType> NameToType = new Dictionary<string, DoorType>(StringComparer.OrdinalIgnoreCase)
        {
            { "049_GATE", DoorType.Scp049Gate },
            { "049_ARMORY", DoorType.Scp049Armory },
            { "079_FIRST", DoorType.Scp079First },
            { "079_SECOND", DoorType.Scp079Second },
            { "079_ARMORY", DoorType.Scp079Armory },
            { "096", DoorType.Scp096 },
            { "106_PRIMARY", DoorType.Scp106Primary },
            { "106_SECONDARY", DoorType.Scp106Secondary },
            { "173_GATE", DoorType.Scp173Gate },
            { "173_CONNECTOR", DoorType.Scp173Connector },
            { "173_ARMORY", DoorType.Scp173Armory },
            { "173_BOTTOM", DoorType.Scp173Bottom },
            { "GR18_INNER", DoorType.GR18Inner },
            { "GR18_GATE", DoorType.GR18Gate },
            { "914_DOOR", DoorType.Scp914Door },
            { "914_GATE", DoorType.Scp914Gate },
            { "939_CRYO", DoorType.Scp939Cryo },
            { "CHECKPOINT_LCZ_A", DoorType.CheckpointLczA },
            { "CHECKPOINT_LCZ_B", DoorType.CheckpointLczB },
            { "ENTRANCE_DOOR", DoorType.EntranceDoor },
            { "ESCAPE_PRIMARY", DoorType.EscapePrimary },
            { "ESCAPE_SECONDARY", DoorType.EscapeSecondary },
            { "SERVERS_BOTTOM", DoorType.ServersBottom },
            { "GATE_A", DoorType.GateA },
            { "GATE_B", DoorType.GateB },
            { "HCZ_ARMORY", DoorType.HczArmory },
            { "HEAVY_CONTAINMENT_DOOR", DoorType.HeavyContainmentDoor },
            { "HID", DoorType.HID },
            { "HID_LEFT", DoorType.HIDLeft },
            { "HID_RIGHT", DoorType.HIDRight },
            { "INTERCOM", DoorType.Intercom },
            { "LCZ_ARMORY", DoorType.LczArmory },
            { "LCZ_CAFE", DoorType.LczCafe },
            { "LCZ_WC", DoorType.LczWc },
            { "LIGHT_CONTAINMENT_DOOR", DoorType.LightContainmentDoor },
            { "NUKE_ARMORY", DoorType.NukeArmory },
            { "NUKE_SURFACE", DoorType.NukeSurface },
            { "PRISON_DOOR", DoorType.PrisonDoor },
            { "SURFACE_GATE", DoorType.SurfaceGate },
            { "330", DoorType.Scp330 },
            { "330_CHAMBER", DoorType.Scp330Chamber },
            { "CHECKPOINT_GATE", DoorType.CheckpointGate },
            { "SURFACE_DOOR", DoorType.SurfaceDoor },
            { "CHECKPOINT_EZ_HCZ_A", DoorType.CheckpointEzHczA },
            { "CHECKPOINT_EZ_HCZ_B", DoorType.CheckpointEzHczB },
            { "UNKNOWN_GATE", DoorType.UnknownGate },
            { "UNKNOWN_ELEVATOR", DoorType.UnknownElevator },
            { "ELEVATOR_GATE_A", DoorType.ElevatorGateA },
            { "ELEVATOR_GATE_B", DoorType.ElevatorGateB },
            { "ELEVATOR_NUKE", DoorType.ElevatorNuke },
            { "ELEVATOR_SCP049", DoorType.ElevatorScp049 },
            { "ELEVATOR_LCZA", DoorType.ElevatorLczA },
            { "ELEVATOR_LCZB", DoorType.ElevatorLczB },
            { "CHECKPOINT_ARMORY_A", DoorType.CheckpointArmoryA },
            { "CHECKPOINT_ARMORY_B", DoorType.CheckpointArmoryB },
            { "AIRLOCK", DoorType.Airlock },
            { "173_NEW_GATE", DoorType.Scp173NewGate },
        };

        public static DoorType GetDoorType(Interactables.Interobjects.DoorUtils.DoorVariant variant)
        {
            if (variant == null || string.IsNullOrEmpty(variant.DoorName))
                return DoorType.UnknownDoor;

            string key = variant.DoorName.Replace(" ", "_").ToUpperInvariant();
            return NameToType.TryGetValue(key, out var type) ? type : DoorType.UnknownDoor;
        }
    }
}