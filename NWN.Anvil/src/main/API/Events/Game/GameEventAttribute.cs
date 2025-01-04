using System;
using JetBrains.Annotations;

namespace Anvil.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(IEvent))]
  internal sealed class GameEventAttribute(EventScriptType eventScriptType) : Attribute
  {
    /// <summary>
    /// The native event type for this event.
    /// </summary>
    public readonly EventScriptType EventScriptType = eventScriptType;
  }
}
