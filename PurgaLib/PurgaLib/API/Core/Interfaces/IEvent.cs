namespace PurgaLib.API.Core.Interfaces;

public interface IEvent
{
    public abstract void Register();
    public abstract void UnRegister();
}