using System;
using System.Collections.Generic;
using PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibAPI.Features.Server;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute
{
    public sealed class Event<T> : IEvent
    {
        private readonly List<Action<T>> _handlers = new();
        
        public void Add(Action<T> handler)
        {
            if (handler == null) return;
            _handlers.Add(handler);
        }

        public void Remove(Action<T> handler)
        {
            if (handler == null) return;
            _handlers.Remove(handler);
        }
        
        public void Invoke(T args)
        {
            foreach (var handler in _handlers)
            {
                try
                {
                    handler(args);
                }
                catch (Exception ex)
                {
                    Log.Error($"[PurgaLib] Event error: {ex}");
                }
            }
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
    }
}
