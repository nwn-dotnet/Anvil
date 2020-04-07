using System;
using JetBrains.Annotations;
using NWM.API.Events;

namespace NWM.API.NWNX.Events
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