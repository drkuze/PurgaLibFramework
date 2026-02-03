using System;
using PurgaLib.Events.EventArgs.SCP;

namespace PurgaLib.Events.Handlers;

public class SCPsHandlers
{
    public static event Action<UpgradingPlayersEventArgs> Upgrading;
    
    public static void InvokeSafely(UpgradingPlayersEventArgs ev) => Upgrading?.Invoke(ev);
}