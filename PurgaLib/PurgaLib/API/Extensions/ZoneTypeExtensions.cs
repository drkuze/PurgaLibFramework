using MapGeneration;
using PurgaLib.API.Enums;

namespace PurgaLib.API.Extensions
{
    public static class ZoneTypeExtensions
    {
        public static ZoneType FromFacility(FacilityZone zone)
        {
            return zone switch
            {
                FacilityZone.LightContainment => ZoneType.LightContainment,
                FacilityZone.HeavyContainment => ZoneType.HeavyContainment,
                FacilityZone.Entrance => ZoneType.Entrance,
                FacilityZone.Surface => ZoneType.Surface,
                _ => ZoneType.Other
            };
        }
        public static FacilityZone ToFacilityZone(this ZoneType zone)
        {
            return zone switch
            {
                ZoneType.LightContainment => FacilityZone.LightContainment,
                ZoneType.HeavyContainment => FacilityZone.HeavyContainment,
                ZoneType.Entrance => FacilityZone.Entrance,
                ZoneType.Surface => FacilityZone.Surface,
                _ => FacilityZone.None
            };
        }

        public static ZoneType ToZoneType(this FacilityZone zone)
        {
            return zone switch
            {
                FacilityZone.LightContainment => ZoneType.LightContainment,
                FacilityZone.HeavyContainment => ZoneType.HeavyContainment,
                FacilityZone.Entrance => ZoneType.Entrance,
                FacilityZone.Surface => ZoneType.Surface,
                _ => ZoneType.Other
            };
        }
    }
}
