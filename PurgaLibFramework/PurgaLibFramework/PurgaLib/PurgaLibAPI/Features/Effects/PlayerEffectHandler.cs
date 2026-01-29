using CustomPlayerEffects;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Enums;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Effects
{
    public class PlayerEffectHandler
    {
        private readonly ReferenceHub _hub;

        public PlayerEffectHandler(ReferenceHub hub)
        {
            _hub = hub;
        }

        public bool TryGetEffect(EffectType type, out StatusEffectBase effect)
        {
            effect = null;

            if (type == EffectType.None)
                return false;

            var name = type.ToEffectName();
            if (string.IsNullOrEmpty(name))
                return false;

            return _hub.playerEffectsController.TryGetEffect(name, out effect);
        }

    }
}
