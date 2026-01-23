using System;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.EventArgs.Map;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events.Handlers;

public static class MapHandlers
{
    public static event Action<DoorInteractingEventArgs> DoorInteracting;
    public static event Action<ElevatorUsingEventArgs> ElevatorUsing;
    public static event Action<OnInteractingTeslaEventArgs> TeslaInteracting;
    
    public static void InvokeSafely(DoorInteractingEventArgs ev) => DoorInteracting?.Invoke(ev);
    public static void InvokeSafely(ElevatorUsingEventArgs ev) => ElevatorUsing?.Invoke(ev);
    public static void InvokeSafely(OnInteractingTeslaEventArgs ev) => TeslaInteracting?.Invoke(ev);
}