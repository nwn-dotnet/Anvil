using System;
using JetBrains.Annotations;
using NWN.API.Constants;

namespace NWN.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(IEvent))]
  public class NativeEventAttribute : Attribute
  {
    /// <summary>
    /// The native event type for this event.
    /// </summary>
    public readonly EventScriptType EventScriptType;

    public NativeEventAttribute(EventScriptType eventScriptType)
    {
      this.EventScriptType = eventScriptType;
    }
  }
}
