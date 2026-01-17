using System;
using System.Collections.Generic;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;

namespace PurgaLibEvents.PurgaLibEvent.Attribute
{
    public sealed class Event<T> : IEvent
    {
        private readonly List<Action<T>> _handlers = new();
        private readonly object _lock = new();

        public void Add(Action<T> handler)
        {
            if (handler == null)
                return;

            lock (_lock)
                _handlers.Add(handler);
        }

        public void Remove(Action<T> handler)
        {
            if (handler == null)
                return;

            lock (_lock)
                _handlers.Remove(handler);
        }

        public void Invoke(T args)
        {
            Action<T>[] snapshot;

            lock (_lock)
                snapshot = _handlers.ToArray();

            foreach (var handler in snapshot)
            {
                try
                {
                    handler(args);
                }
                catch (Exception ex)
                {
                    Log.Error($"[PurgaLib] Event handler error: {ex}");
                }
            }
        }

        void IEvent.Add(Action<object> handler)
        {
            if (handler is Action<T> typed)
                Add(typed);
        }

        void IEvent.Remove(Action<object> handler)
        {
            if (handler is Action<T> typed)
                Remove(typed);
        }

        void IEvent.Invoke(object args)
        {
            if (args is T typed)
                Invoke(typed);
        }

        public static Event<T> operator +(Event<T> ev, Action<T> handler)
        {
            ev.Add(handler);
            return ev;
        }

        public static Event<T> operator -(Event<T> ev, Action<T> handler)
        {
            ev.Remove(handler);
            return ev;
        }
    }
}
