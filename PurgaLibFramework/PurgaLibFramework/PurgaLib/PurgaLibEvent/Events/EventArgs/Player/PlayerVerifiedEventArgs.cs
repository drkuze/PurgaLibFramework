using System;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventSystem.Interfaces;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerVerifiedEventArgs : IEventArgs
    {
        public global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }

        public PlayerVerifiedEventArgs(global::PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player)
        {
            Player = player  ?? throw new ArgumentNullException(nameof(player));
            
        }

        public bool IsAllowed { get; set; }
    }
}