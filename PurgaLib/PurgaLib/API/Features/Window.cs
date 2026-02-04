using System;
using System.Collections.Generic;
using System.Linq;
using Footprinting;
using PlayerStatsSystem;
using PurgaLib.API.Core.Interfaces;
using UnityEngine;
using PurgaLib.API.Enums;
using PurgaLib.API.Features.Server;

namespace PurgaLib.API.Features
{
    public class Window : IWorldObject
    {
        internal static readonly Dictionary<BreakableWindow, Window> Wrappers = new();

        internal Window(BreakableWindow baseWindow, Room room)
        {
            Base = baseWindow;
            Room = room;
            Type = GetGlassType();

            Wrappers[baseWindow] = this;

            if (Type == GlassType.Unknown)
                Logged.Debug($"[GLASS UNKNOWN] Room={Room} Base={Base?.name} HP={Base?.Health}");
        }

        public static IReadOnlyCollection<Window> List => Wrappers.Values;

        public BreakableWindow Base { get; }

        public GameObject GameObject => Base.gameObject;

        public Transform Transform => Base.transform;

        public Room Room { get; }

        public GlassType Type { get; }

        public ZoneType Zone => Room?.Zone ?? ZoneType.Unspecified;

        public Vector3 Position
        {
            get => Transform.position;
            set => Transform.position = value;
        }

        public Quaternion Rotation
        {
            get => Transform.rotation;
            set => Transform.rotation = value;
        }

        public bool IsBreakable => !Base.IsBroken;

        public bool IsBroken
        {
            get => Base.IsBroken;
            set => Base.IsBroken = value;
        }

        public float Health
        {
            get => Base.Health;
            set => Base.Health = value;
        }

        public bool DisableScpDamage
        {
            get => Base._preventScpDamage;
            set => Base._preventScpDamage = value;
        }

        public Player LastAttacker
        {
            get => Player.Get(Base.LastAttacker.Hub);
            set => Base.LastAttacker = value?.Footprint ?? new Footprint();
        }

        public static Window Get(BreakableWindow window)
        {
            if (window == null) return null;
            return Wrappers.TryGetValue(window, out var w) ? w : new Window(window, window.GetComponentInParent<Room>());
        }

        public static IEnumerable<Window> Get(Func<Window, bool> predicate) => List.Where(predicate);

        public static bool TryGet(BreakableWindow window, out Window result)
        {
            result = Get(window);
            return result != null;
        }

        public static bool TryGet(Func<Window, bool> predicate, out IEnumerable<Window> results)
        {
            results = Get(predicate);
            return results.Any();
        }

        public void Break() => Base.ServerDamageWindow(Health);

        public void Damage(float amount) => Base.ServerDamageWindow(amount);

        public void Damage(float amount, DamageHandlerBase handler) => Base.Damage(amount, handler, Vector3.zero);

        public override string ToString() => $"{Type} (HP={Health}) Broken={IsBroken} DisableScp={DisableScpDamage}";

        private GlassType GetGlassType()
        {
            string name = Base.name;

            return name switch
            {
                "B272sa" => Room?.Type switch
                {
                    RoomType.LczGlassBox => GlassType.GR18,
                    RoomType.Lcz330 => GlassType.Scp330,
                    _ => GlassType.Unknown
                },
                "GLASS" => Room?.Type switch
                {
                    RoomType.Hcz079 => GlassType.Scp079,
                    RoomType.HczHid => GlassType.HidMicro,
                    RoomType.HczEzCheckpointA => GlassType.HczEzCheckpointA,
                    RoomType.HczEzCheckpointB => GlassType.HczEzCheckpointB,
                    RoomType.EzGateA when Base.name[7] == '5' => GlassType.GateAArmory,
                    RoomType.EzGateA => GlassType.GateAPit,
                    RoomType.HczLoadingBay => GlassType.HczLoadingBay,
                    _ => GlassType.Unknown
                },
                "Window" => Room?.Type switch
                {
                    RoomType.Hcz049 => GlassType.Scp049,
                    RoomType.Hcz127 => GlassType.Scp127,
                    RoomType.HczHid => GlassType.HidMicro,
                    RoomType.HczTestRoom => GlassType.TestRoom,
                    _ => GlassType.Unknown
                },
                "Glass" => Room?.Type switch
                {
                    RoomType.Hcz079 => GlassType.Scp079Trigger,
                    RoomType.HczHid => GlassType.HidMicro,
                    _ => GlassType.Unknown
                },
                "glass" => GlassType.Scp079,
                "VTGLASS" => GlassType.Plants,
                _ => GlassType.Unknown
            };
        }
    }
}
