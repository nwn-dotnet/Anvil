using System;
using System.Collections.Generic;
using System.Linq;
using NWN.API;
using NWN.API.Events;
using NWN.Services;
using NWNX.API.Events;

namespace NWNX.Services
{
  [ServiceBinding(typeof(IScriptDispatcher))]
  [ServiceBinding(typeof(NWNXEventService))]
  public sealed class NWNXEventService : IScriptDispatcher
  {
    // Type info cache
    private readonly Dictionary<Type, IEventAttribute> cachedEventInfo = new Dictionary<Type, IEventAttribute>();

    // Lookup Data
    private readonly Dictionary<string, NWNXEvent> scriptToEventMap = new Dictionary<string, NWNXEvent>();
    private readonly Dictionary<Type, NWNXEvent> typeToHandlerMap = new Dictionary<Type, NWNXEvent>();

    /// <summary>
    /// Subscribes to the specified event.
    /// </summary>
    /// <param name="handler">The callback function/handler for this event.</param>
    /// <typeparam name="TEvent">The event to subscribe to.</typeparam>
    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : NWNXEvent<TEvent>, new()
    {
      TEvent eventHandler = GetOrCreateHandler<TEvent>();
      eventHandler.Callbacks += handler;
    }

    /// <summary>
    /// Removes an existing global event handler that was added using <see cref="Subscribe{TEvent}"/>.
    /// </summary>
    /// <param name="existingHandler">The existing handler/callback.</param>
    /// <typeparam name="TEvent">The event to unsubscribe from.</typeparam>
    public void Unsubscribe<TEvent>(Action<TEvent> existingHandler) where TEvent : NWNXEvent<TEvent>
    {
      if (typeToHandlerMap.TryGetValue(typeof(TEvent), out NWNXEvent eventHandler))
      {
        TEvent handler = (TEvent)eventHandler;
        handler.Callbacks -= existingHandler;

        if (!handler.HasSubscribers)
        {
          RemoveHandler(typeof(TEvent), handler.ScriptName);
        }
      }
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      if (scriptToEventMap.TryGetValue(scriptName, out NWNXEvent eventHandler))
      {
        eventHandler.ProcessEvent(oidSelf.ToNwObject());
        return ScriptHandleResult.Handled;
      }

      return ScriptHandleResult.NotHandled;
    }

    private TEvent GetOrCreateHandler<TEvent>() where TEvent : NWNXEvent<TEvent>, new()
    {
      Type type = typeof(TEvent);
      if (typeToHandlerMap.TryGetValue(type, out NWNXEvent eventHandler))
      {
        return (TEvent)eventHandler;
      }

      return CreateEventHandler<TEvent>();
    }

    private IEventAttribute GetEventInfo(Type type)
    {
      if (cachedEventInfo.TryGetValue(type, out IEventAttribute eventAttribute))
      {
        return eventAttribute;
      }

      eventAttribute = LoadEventInfo(type);
      cachedEventInfo[type] = eventAttribute;

      return eventAttribute;
    }

    private IEventAttribute LoadEventInfo(Type type)
    {
      object eventAttribute = type.GetCustomAttributes(typeof(IEventAttribute), true).SingleOrDefault();
      if (eventAttribute != null)
      {
        return (IEventAttribute) eventAttribute;
      }

      throw new InvalidOperationException($"Event Type {type.GetFullName()} does not define an event info attribute!");
    }

    private TEvent CreateEventHandler<TEvent>() where TEvent : NWNXEvent<TEvent>, new()
    {
      TEvent eventHandler = new TEvent();
      string scriptName = ScriptNameGenerator.Create();

      eventHandler.ScriptName = scriptName;

      IEventAttribute eventInfo = GetEventInfo(typeof(TEvent));
      eventInfo.InitHook(scriptName);

      scriptToEventMap[scriptName] = eventHandler;
      typeToHandlerMap[typeof(TEvent)] = eventHandler;

      return eventHandler;
    }

    private void RemoveHandler(Type type, string scriptName)
    {
      typeToHandlerMap.Remove(type);
      scriptToEventMap.Remove(scriptName);
    }
  }
}
