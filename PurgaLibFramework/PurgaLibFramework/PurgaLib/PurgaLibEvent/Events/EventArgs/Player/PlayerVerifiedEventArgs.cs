using System;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    public class PlayerVerifiedEventArgs : System.EventArgs
    {
        public LabApi.Features.Wrappers.Player Player { get; }

        public PlayerVerifiedEventArgs(LabApi.Features.Wrappers.Player player)
        {
            Player = player  ?? throw new ArgumentNullException(nameof(player));
        }
    }
}