using System;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute
{
    public interface IEvent
    {
        void Add(Action<object> handler);
        void Remove(Action<object> handler);
        void Invoke(object args);
    }
}
