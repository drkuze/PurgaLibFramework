using System.Collections.Generic;
using System.Linq;
using PlayerRoles;
using UnityEngine;

namespace PurgaLib.API.Features
{
    /// <summary>
    /// Wrapper completo per TeslaGate in stile PurgaLib.
    /// </summary>
    public class TeslaGate
    {
        private static readonly Dictionary<global::TeslaGate, TeslaGate> BaseToWrapper = new();
        
        public static IReadOnlyCollection<TeslaGate> List => BaseToWrapper.Values;
        
        public static HashSet<Player> IgnoredPlayers { get; set; } = new();
        
        public static List<RoleTypeId> IgnoredRoles { get; set; } = new();
        public static List<Team> IgnoredTeams { get; set; } = new();
        
        public global::TeslaGate Base { get; }
        
        public Vector3 Position => Base.transform.position;
        
        public Room Room { get; }
        
        internal TeslaGate(global::TeslaGate baseTeslaGate, Room room)
        {
            Base = baseTeslaGate;
            Room = room;
            BaseToWrapper[baseTeslaGate] = this;
        }
        
        public static TeslaGate Get(global::TeslaGate baseTeslaGate)
        {
            if (BaseToWrapper.TryGetValue(baseTeslaGate, out var wrapper))
                return wrapper;
            
            Room room = Room.List.FirstOrDefault(r => r.Contains(baseTeslaGate.transform.position)) ?? Room.Random();

            return new TeslaGate(baseTeslaGate, room);
        }
        
        public void Trigger(bool instant = false)
        {
            if (instant) Base.RpcInstantBurst();
            else Base.ServerSideCode();
        }
        
        public void ForceTrigger()
        {
            Base.RpcPlayAnimation();
        }
        
        public bool IsShocking => Base.InProgress;
        
        public float InactiveTime
        {
            get => Base.NetworkInactiveTime;
            set => Base.NetworkInactiveTime = value;
        }
        
        public Vector3 HurtRange
        {
            get => Base.sizeOfKiller;
            set => Base.sizeOfKiller = value;
        }
        
        public float TriggerRange
        {
            get => Base.sizeOfTrigger;
            set => Base.sizeOfTrigger = value;
        }
        
        public float IdleRange
        {
            get => Base.distanceToIdle;
            set => Base.distanceToIdle = value;
        }
        
        public float ActivationTime
        {
            get => Base.windupTime;
            set => Base.windupTime = value;
        }
        
        public float CooldownTime
        {
            get => Base.cooldownTime;
            set => Base.cooldownTime = value;
        }
        
        public bool UseInstantBurst
        {
            get => Base.next079burst;
            set => Base.next079burst = value;
        }

        public IEnumerable<Player> PlayersInHurtRange => Player.List.Where(IsPlayerInHurtRange);
        
        public IEnumerable<Player> PlayersInIdleRange => Player.List.Where(IsPlayerInIdleRange);

        public IEnumerable<Player> PlayersInTriggerRange => Player.List.Where(IsPlayerInTriggerRange);


        public bool IsPlayerInHurtRange(Player player)
        {
            return player != null && Base.killers.Any(x => new Bounds(x.transform.position, Base.sizeOfKiller).Contains(player.Position));
        }

        public bool IsPlayerInIdleRange(Player player)
        {
            return player != null && Base.IsInIdleRange(player.ReferenceHub);
        }
        
        public bool IsPlayerInTriggerRange(Player player)
        {
            return player != null && Base.PlayerInRange(player.ReferenceHub);
        }
        
        public bool CanBeTriggered(Player player)
        {
            if (player == null || !player.IsAlive)
                return false;

            if (IgnoredPlayers.Contains(player) || IgnoredRoles.Contains(player.Role.Type) || IgnoredTeams.Contains(player.Role.Team))
                return false;

            return IsPlayerInTriggerRange(player);
        }
    }
}
