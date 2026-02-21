using System;
using PurgaLib.Events.EventArgs.Map;

namespace PurgaLib.Events.Handlers;

public static class MapHandlers
{
    public static event Action<OnInteractingTeslaEventArgs> TeslaInteracting;
    public static event Action<UpgradingPlayersEventArgs> Upgrading;

    public static void InvokeSafely(OnInteractingTeslaEventArgs ev) => TeslaInteracting?.Invoke(ev);
    public static void InvokeSafely(UpgradingPlayersEventArgs ev) => Upgrading?.Invoke(ev);
}