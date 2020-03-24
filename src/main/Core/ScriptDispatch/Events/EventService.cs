using System;
using System.Collections.Generic;
using NLog;
using NWM.API;
using EventHandler = NWM.API.EventHandler;

namespace NWM.Core
{
  [Service(typeof(IScriptDispatcher), IsCollection = true)]
  public class EventService : IScriptDispatcher
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private List<EventHandler> registeredHandlers = new List<EventHandler>();

    public T CreateEventHandler<T, TEventType>(Dictionary<string, TEventType> scriptMap) where T : API.EventHandler<TEventType>, new() where TEventType : Enum
    {
      T newHandler = new T();
      newHandler.Init(scriptMap);
      RegisterEventHandler(newHandler);

      return newHandler;
    }

    public T GetEventHandler<T>(string scriptNamePrefix) where T : EventHandler, new()
    {
      foreach (EventHandler eventHandler in registeredHandlers)
      {
        if (eventHandler.ScriptPrefix == scriptNamePrefix && eventHandler is T tEventHandler)
        {
          return tEventHandler;
        }
      }

      T newHandler = new T();
      newHandler.Init(scriptNamePrefix);
      RegisterEventHandler(newHandler);

      return newHandler;
    }

    private void RegisterEventHandler(EventHandler eventHandler)
    {
      Log.Info($"Registering event handler ({eventHandler.GetType().FullName}) with prefix: ({eventHandler.ScriptPrefix})");
      registeredHandlers.Add(eventHandler);
    }

    public int ExecuteScript(string scriptName, uint oidSelf)
    {
      foreach (EventHandler eventHandler in registeredHandlers)
      {
        if (eventHandler.ProcessScriptEvent(scriptName, oidSelf.ToNwObject()))
        {
          return ScriptDispatchConstants.SCRIPT_HANDLED;
        }
      }

      return ScriptDispatchConstants.SCRIPT_NOT_HANDLED;
    }
  }
}