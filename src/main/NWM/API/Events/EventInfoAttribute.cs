using System;
using JetBrains.Annotations;

namespace NWM.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(IEvent<>))]
  public class EventInfoAttribute : Attribute
  {
    public string DefaultScriptSuffix;
    public string EventName;
    public EventType EventType;

    public EventInfoAttribute(EventType eventType)
    {
      this.EventType = eventType;
    }
  }
}