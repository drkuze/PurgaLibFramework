using PurgaLib.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PurgaLib.API.Features.Server;

namespace PurgaLib.API.Core
{
    public class PEventRegister : PComponent
    {
        private readonly List<IEvent> _events = new();

        public void RegisterAssembly(Assembly assembly)
        {
            if (assembly == null) return;

            var types = assembly.GetTypes()
                .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in types)
            {
                try
                {
                    var instance = (IEvent)Activator.CreateInstance(type);
                    instance.Register();
                    _events.Add(instance);

                    Logged.Info($"[AutoRegister] -> [{assembly.GetName().Name}] Event Registered: {type.FullName}");
                }
                catch (Exception ex)
                {
                    Logged.Error($"[AutoRegister] Error while registering: {type.FullName}: {ex}");
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
}
