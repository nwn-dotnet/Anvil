using System;
using JetBrains.Annotations;
using NWM.API.Constants;

namespace NWM.API.Events
{
  [AttributeUsage(AttributeTargets.Class)]
  [BaseTypeRequired(typeof(IEvent<>))]
  public class ScriptEventAttribute : Attribute
  {
    public EventScriptType EventScriptType;

    public ScriptEventAttribute(EventScriptType eventScriptType)
    {
      this.EventScriptType = eventScriptType;
    }
  }
}