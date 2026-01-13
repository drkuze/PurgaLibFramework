using System;
using LabApi.Events.Handlers;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handler
{
    public static class DoorHandler
    {
        private static bool _initialized;

        public static event Action<DoorInteractingEventArgs> Interacting;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            PlayerEvents.InteractingDoor -= OnInteractingDoor;
            PlayerEvents.InteractingDoor += OnInteractingDoor;
        }

        private static void OnInteractingDoor(LabApi.Events.Arguments.PlayerEvents.PlayerInteractingDoorEventArgs ev)
        {
            var player = Player.Get(ev.Player);
            var door = Door.Get(ev.Door);

            var args = new DoorInteractingEventArgs(player, door)
            {
                IsAllowed = ev.IsAllowed
            };

            Interacting?.Invoke(args);

            ev.IsAllowed = args.IsAllowed;
        }

        public static void AddHandler(Action<DoorInteractingEventArgs> handler)
            => Interacting += handler;

        public static void RemoveHandler(Action<DoorInteractingEventArgs> handler)
            => Interacting -= handler;
    }
}
