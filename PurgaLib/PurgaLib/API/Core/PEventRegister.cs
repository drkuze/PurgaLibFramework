using PurgaLib.API.Core.Interfaces;
using PurgaLib.API.Features.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PurgaLib.API.Core;

public class PEventRegister : PComponent
{
    private readonly List<IEvent> _events = new();

    public void RegisterAll()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        var types = assembly.GetTypes()
            .Where(t =>
                typeof(IEvent).IsAssignableFrom(t) &&
                !t.IsInterface &&
                !t.IsAbstract
            );

        foreach (var type in types)
        {
            try
            {
                IEvent instance = (IEvent)Activator.CreateInstance(type);
                instance.Register();
                _events.Add(instance);

                Logged.Info($"[AutoRegister] Event Registered: {type.Name}");
            }
            catch (Exception ex)
            {
                Logged.Error($"[AutoRegister] Error while registering: {type.Name}: {ex}");
            }
        }
    }

    public void UnRegisterAll()
    {
        foreach (var ev in _events)
        {
            try
            {
                ev.UnRegister();
            }
            catch (Exception ex)
            {
                Logged.Error($"[AutoRegister] Error UnRegistering: {ev.GetType().Name}: {ex}");
            }
        }
        _events.Clear();
    }
}