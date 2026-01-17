using System;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class PurgaLibEventPatcher : System.Attribute
    {
        private readonly Type handlerType;
        private readonly string eventName;
        
        internal PurgaLibEventPatcher(Type handlerType, string eventName)
        {
            this.handlerType = handlerType;
            this.eventName = eventName;
        }
        
        internal IEvent Event => (IEvent)handlerType.GetProperty(eventName)?.GetValue(null);
    }
