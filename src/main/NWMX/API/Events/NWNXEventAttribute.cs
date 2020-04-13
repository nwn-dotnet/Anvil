using System;
using JetBrains.Annotations;
using NWM.API.Events;

namespace NWMX.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(IEvent<>))]
  public class NWNXEventAttribute : Attribute
  {
    public readonly string EventName;

    public NWNXEventAttribute(string eventName)
    {
      this.EventName = eventName;
    }
  }
}