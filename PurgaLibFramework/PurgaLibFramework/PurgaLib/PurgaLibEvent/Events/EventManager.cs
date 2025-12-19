using System;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Events
{
    public static class EventManager
    {
        public static void Invoke<T>(EventHandler<T> handler, object sender, T ev)
            where T : System.EventArgs
        {
            handler?.Invoke(sender, ev);
        }
    }
}