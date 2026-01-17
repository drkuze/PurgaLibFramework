using System;

public interface IEvent
{
    void Add(Action<object> handler);
    void Remove(Action<object> handler);
    void Invoke(object args);
}
