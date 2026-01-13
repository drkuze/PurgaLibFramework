using System;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibEvent.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal sealed class EventPatchAttribute : System.Attribute
    {
        private readonly Type _handlerType;
        private readonly string _eventName;

        internal EventPatchAttribute(Type handlerType, string eventName)
        {
            _handlerType = handlerType;
            _eventName = eventName;
        }

        internal IEvent Event
        {
            get
            {
                var prop = _handlerType.GetProperty(
                    _eventName,
                    System.Reflection.BindingFlags.Static |
                    System.Reflection.BindingFlags.Public);

                return prop?.GetValue(null) as IEvent;
            }
        }
    }
}
