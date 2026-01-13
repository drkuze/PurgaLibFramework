using System;
using LabApi.Events.Handlers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class ElevatorHandler
    {
        private static bool _initialized;

        public static event Action<ElevatorUsingEventArgs> Interacting;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            PlayerEvents.InteractingElevator += ev =>
            {
                var player = Player.Get(ev.Player);
                var args = new ElevatorUsingEventArgs(player, ev.Elevator)
                {
                    IsAllowed = true
                };

                Interacting?.Invoke(args);

                ev.IsAllowed = args.IsAllowed;
            };
        }
    }
}
