using System;
using Anvil.API;
using JetBrains.Annotations;
using NWN.API.Constants;

namespace NWN.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(IEvent))]
  internal sealed class GameEventAttribute : Attribute
  {
    /// <summary>
    /// The native event type for this event.
    /// </summary>
    public readonly EventScriptType EventScriptType;

    public GameEventAttribute(EventScriptType eventScriptType)
    {
      EventScriptType = eventScriptType;
    }
  }
}
