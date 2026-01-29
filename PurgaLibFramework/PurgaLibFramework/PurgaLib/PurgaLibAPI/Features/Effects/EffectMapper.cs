using CustomPlayerEffects;
using InventorySystem.Items.Usables.Scp244.Hypothermia;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Enums;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Effects
{
    public static class EffectMapper
    {
        public static string ToEffectName(this EffectType type)
        {
            return type switch
            {
                EffectType.AmnesiaItems => nameof(AmnesiaItems),
                EffectType.AmnesiaVision => nameof(AmnesiaVision),
                EffectType.Asphyxiated => nameof(Asphyxiated),
                EffectType.Bleeding => nameof(Bleeding),
                EffectType.Blinded => nameof(Blindness),
                EffectType.Burned => nameof(Burned),
                EffectType.Concussed => nameof(Concussed),
                EffectType.Corroding => nameof(Corroding),
                EffectType.Deafened => nameof(Deafened),
                EffectType.Decontaminating => nameof(Decontaminating),
                EffectType.Disabled => nameof(Disabled),
                EffectType.Ensnared => nameof(Ensnared),
                EffectType.Exhausted => nameof(Exhausted),
                EffectType.Flashed => nameof(Flashed),
                EffectType.Hemorrhage => nameof(Hemorrhage),
                EffectType.Invigorated => nameof(Invigorated),
                EffectType.BodyshotReduction => nameof(BodyshotReduction),
                EffectType.Poisoned => nameof(Poisoned),
                EffectType.Scp207 => nameof(Scp207),
                EffectType.Invisible => nameof(Invisible),
                EffectType.SinkHole => nameof(Sinkhole),
                EffectType.DamageReduction => nameof(DamageReduction),
                EffectType.MovementBoost => nameof(MovementBoost),
                EffectType.RainbowTaste => nameof(RainbowTaste),
                EffectType.SeveredHands => nameof(SeveredHands),
                EffectType.Stained => nameof(Stained),
                EffectType.Vitality => nameof(Vitality),
                EffectType.Hypothermia => nameof(Hypothermia),
                EffectType.Scp1853 => nameof(Scp1853),
                EffectType.CardiacArrest => nameof(CardiacArrest),
                EffectType.InsufficientLighting => nameof(InsufficientLighting),
                EffectType.SoundtrackMute => nameof(SoundtrackMute),
                EffectType.SpawnProtected => nameof(SpawnProtected),
                EffectType.Traumatized => nameof(Traumatized),
                EffectType.AntiScp207 => nameof(AntiScp207),
                EffectType.Scanned => nameof(Scanned),
                EffectType.PocketCorroding => nameof(PocketCorroding),
                EffectType.SilentWalk => nameof(SilentWalk),
                EffectType.Strangled => nameof(Strangled),
                EffectType.Ghostly => nameof(Ghostly),
                EffectType.FogControl => nameof(FogControl),
                EffectType.Slowness => nameof(Slowness),
                _ => null
            };
        }
    }
}
