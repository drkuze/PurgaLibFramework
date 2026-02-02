using PurgaLib.API.Enums;

namespace PurgaLib.API.Features.Effects
{
    public class Effect
    {
        public Effect() { }

        public Effect(
            EffectType type,
            float duration,
            byte intensity = 1,
            bool addDurationIfActive = false,
            bool isEnabled = true)
        {
            Type = type;
            Duration = duration;
            Intensity = intensity;
            AddDurationIfActive = addDurationIfActive;
            IsEnabled = isEnabled;
        }

        public EffectType Type { get; set; }

        public float Duration { get; set; }

        public byte Intensity { get; set; }

        public bool AddDurationIfActive { get; set; }

        public bool IsEnabled { get; set; }

        public override string ToString()
            => $"({Type}) Duration={Duration} Intensity={Intensity} Add={AddDurationIfActive}";
    }
}
