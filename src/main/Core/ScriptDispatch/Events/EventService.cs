using System.Collections.Generic;
using NLog;
using NWM.API;

namespace NWM.Core
{
  [Service(typeof(IScriptDispatcher), IsCollection = true)]
  public class EventService : IScriptDispatcher
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private List<EventHandler> registeredHandlers = new List<EventHandler>();

    public T GetEventHandler<T>(string scriptNamePrefix) where T : EventHandler, new()
    {
      foreach (EventHandler eventHandler in registeredHandlers)
      {
        if (eventHandler.NamePrefix == scriptNamePrefix && eventHandler is T tEventHandler)
        {
          return tEventHandler;
        }
      }

      T newHandler = new T {NamePrefix = scriptNamePrefix};
      RegisterEventHandler(newHandler);

      return newHandler;
    }

    private void RegisterEventHandler(EventHandler eventHandler)
    {
      Log.Info($"Registering event handler ({eventHandler.GetType().FullName}) with prefix: ({eventHandler.NamePrefix})");
      registeredHandlers.Add(eventHandler);
    }

    public int ExecuteScript(string scriptName, uint oidSelf)
    {
      foreach (EventHandler eventHandler in registeredHandlers)
      {
        if (!scriptName.StartsWith(eventHandler.NamePrefix))
        {
          continue;
        }

        string eventName = scriptName.Substring(eventHandler.NamePrefix.Length);
        if (eventHandler.HandleScriptEvent(eventName, oidSelf.ToNwObject()))
        {
          return ScriptDispatchConstants.SCRIPT_HANDLED;
        }
      }

      return ScriptDispatchConstants.SCRIPT_NOT_HANDLED;
    }
  }
}