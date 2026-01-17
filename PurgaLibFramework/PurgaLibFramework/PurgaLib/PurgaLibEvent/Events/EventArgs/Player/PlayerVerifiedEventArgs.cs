using System;

namespace PurgaLibEvents.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerVerifiedEventArgs : System.EventArgs
    {
        public PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player Player { get; }

        public PlayerVerifiedEventArgs(PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Player player)
        {
            Player = player  ?? throw new ArgumentNullException(nameof(player));
            
        }
    }
}