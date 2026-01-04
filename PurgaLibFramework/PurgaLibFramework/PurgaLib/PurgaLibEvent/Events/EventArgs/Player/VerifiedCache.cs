using System.Collections.Generic;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Player
{
    internal static class VerifiedPlayersCache
    {
        internal static readonly HashSet<string> Verified = new();
    }
}