using System;
using LabApi.Events.Handlers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class TeslaHandler
    {
        private static bool _initialized;

        public static event Action<OnInteractingTeslaEventArgs> InteractingTesla;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            PlayerEvents.TriggeringTesla -= OnTriggeringTesla;
            PlayerEvents.TriggeringTesla += OnTriggeringTesla;
        }

        private static void OnTriggeringTesla(LabApi.Events.Arguments.PlayerEvents.PlayerTriggeringTeslaEventArgs ev)
        {
            var player = Player.Get(ev.Player);
            var args = new OnInteractingTeslaEventArgs(player)
            {
                IsAllowed = true
            };

            InteractingTesla?.Invoke(args);

            if (!args.IsAllowed)
                ev.IsAllowed = false;
        }

        public static void OnInteracting(OnInteractingTeslaEventArgs ev) 
            => InteractingTesla?.Invoke(ev);
    }
}
