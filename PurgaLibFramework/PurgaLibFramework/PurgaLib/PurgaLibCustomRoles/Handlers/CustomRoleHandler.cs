using System;
using System.Collections.Generic;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using PlayerRoles;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibCustomRoles.Handlers
{
    public static class CustomRoleHandler
    {
        public static readonly List<CustomRole> Registered = new();
        
        private static EventHandler<Player> _assigned;
        private static EventHandler<Player> _removed;

        public static event EventHandler<Player> Assigned
        {
            add { bool wasEmpty = _assigned == null; _assigned += value; if (wasEmpty) RegisterEvents(); }
            remove => _assigned -= value;
        }

        public static event EventHandler<Player> Removed
        {
            add { bool wasEmpty = _removed == null; _removed += value; if (wasEmpty) RegisterEvents(); }
            remove => _removed -= value;
        }
        
        public static void Register(CustomRole role)
        {
            if (role == null || Registered.Contains(role)) return;
            Registered.Add(role);
        }
        
        public static void Give(Player player, CustomRole role)
        {
            role.OnAssign(player);
        }
        
        public static void Remove(Player player, CustomRole role)
        {
            role.OnRemove(player);
        }
        
        public static void RegisterEvents()
        {
            PlayerEvents.Joined += ev =>
            {
                var defaultRole = GetDefaultRole();
                defaultRole.OnAssign(ev.Player);
            };

            Log.Success("[PurgaLib] CustomRoleHandler events registered.");
        }

        private static CustomRole GetDefaultRole()
        {
            return new CustomRole("default", "Player", "", "White", RoleTypeId.ClassD);
        }
        
        internal static void InvokeAssigned(Player player) => _assigned?.Invoke(null, player);
        internal static void InvokeRemoved(Player player) => _removed?.Invoke(null, player);
    }
}
