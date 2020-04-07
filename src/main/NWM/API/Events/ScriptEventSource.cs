using System.Collections.Generic;
using NWM.API.Constants;

namespace NWM.API.Events
{
  public class ScriptEventSource
  {
    public NwObject Object;
    public readonly Dictionary<EventScriptType, IEvent> EventHandlers = new Dictionary<EventScriptType, IEvent>();

    public void CheckScriptEventLinked<T>(EventScriptType eventType, IEvent<T> eventHandler) where T : IEvent<T>
    {


    }
  }
}