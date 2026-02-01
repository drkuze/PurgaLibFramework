using PurgaLib.API.Enums;
using System.Collections.Generic;

namespace PurgaLib.API.Extensions;

public static class FirearmExtensions
{
    private static readonly Dictionary<FirearmType, ItemType> Mapping = new()
        {
            { FirearmType.GunCOM15, ItemType.GunCOM15 },
            { FirearmType.GunCOM18, ItemType.GunCOM18 },
            { FirearmType.GunRevolver, ItemType.GunRevolver },
            { FirearmType.GunShotgun, ItemType.GunShotgun },
            { FirearmType.GunAK, ItemType.GunAK },
            { FirearmType.GunLogicer, ItemType.GunLogicer },
            { FirearmType.GunCrossvec, ItemType.GunCrossvec },
            { FirearmType.GunE11SR, ItemType.GunE11SR },
            { FirearmType.GunFSP9, ItemType.GunFSP9 },
            { FirearmType.GunA7, ItemType.GunA7 },
            { FirearmType.GunFRMG0, ItemType.GunFRMG0 },
            { FirearmType.ParticleDisruptor, ItemType.ParticleDisruptor }
        };

    public static ItemType GetItemType(this FirearmType firearmType)
    {
        return Mapping.TryGetValue(firearmType, out var type) ? type : ItemType.None;
    }
}