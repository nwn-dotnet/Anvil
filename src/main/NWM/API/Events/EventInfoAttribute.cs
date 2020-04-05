using System;
using System.Reflection;
using JetBrains.Annotations;

namespace NWM.API
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