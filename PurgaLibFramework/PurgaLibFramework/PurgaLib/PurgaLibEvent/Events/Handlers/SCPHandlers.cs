using System;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.SCP;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handlers;

public class SCPHandlers
{
    public static event Action<UpgradingPlayersEventArgs> Upgrading;
    
    public static void InvokeSafely(UpgradingPlayersEventArgs ev) => Upgrading?.Invoke(ev);
}