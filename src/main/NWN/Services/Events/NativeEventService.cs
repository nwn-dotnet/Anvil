using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using NWN.API;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;

namespace NWN.Services
{
  [ServiceBinding(typeof(IScriptDispatcher))]
  [ServiceBinding(typeof(NativeEventService))]
  public sealed class NativeEventService : IScriptDispatcher
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Type info cache
    private readonly Dictionary<Type, NativeEventAttribute> cachedEventInfo = new Dictionary<Type, NativeEventAttribute>();

    // Lookup Data
    private readonly Dictionary<string, EventHandler> scriptToEventMap = new Dictionary<string, EventHandler>();
    private readonly Dictionary<Type, EventHandler> typeToHandlerMap = new Dictionary<Type, EventHandler>();

    /// <summary>
    /// Subscribes to the specified event on the given object.
    /// </summary>
    /// <param name="nwObject">The subscribe target for this event.</param>
    /// <param name="handler">The callback function/handler for this event.</param>
    /// <typeparam name="TObject">The type of nwObject.</typeparam>
    /// <typeparam name="TEvent">The event to subscribe to.</typeparam>
    public void Subscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> handler)
      where TEvent : NativeEvent<TObject, TEvent>, new()
      where TObject : NwObject
    {
      EventHandler eventHandler = GetOrCreateHandler<TObject, TEvent>();

      NativeEventAttribute eventInfo = GetEventInfo(typeof(TEvent));
      InitObjectHook<TObject, TEvent>(eventHandler, nwObject, eventInfo.EventScriptType);

      eventHandler.Subscribe(nwObject, handler);
    }

    /// <summary>
    /// Removes an existing event handler from an object that was added using <see cref="Subscribe{TObject,TEvent}"/>.
    /// </summary>
    /// <param name="nwObject">The object containing the existing subscription.</param>
    /// <param name="existingHandler">The existing handler/callback.</param>
    /// <typeparam name="TObject">The type of nwObject.</typeparam>
    /// <typeparam name="TEvent">The event to unsubscribe from.</typeparam>
    public void Unsubscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> existingHandler)
      where TEvent : NativeEvent<TObject, TEvent>, new()
      where TObject : NwObject
    {
      if (typeToHandlerMap.TryGetValue(typeof(TEvent), out EventHandler eventHandler))
      {
        bool canRemove = eventHandler.Unsubscribe(nwObject, existingHandler);
        if (canRemove)
        {
          RemoveHandler(typeof(TEvent), eventHandler.ScriptName);
        }
      }
    }

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string scriptName, uint oidSelf)
    {
      if (scriptToEventMap.TryGetValue(scriptName, out EventHandler eventHandler))
      {
        eventHandler.CallEvents(oidSelf.ToNwObject());
        return ScriptHandleResult.Handled;
      }

      return ScriptHandleResult.NotHandled;
    }

    private EventHandler GetOrCreateHandler<TObject, TEvent>()
      where TEvent : NativeEvent<TObject, TEvent>, new()
      where TObject : NwObject
    {
      if (typeToHandlerMap.TryGetValue(typeof(TEvent), out EventHandler eventHandler))
      {
        return eventHandler;
      }

      return CreateEventHandler<TObject, TEvent>();
    }

    private NativeEventAttribute GetEventInfo(Type type)
    {
      if (cachedEventInfo.TryGetValue(type, out NativeEventAttribute eventAttribute))
      {
        return eventAttribute;
      }

      eventAttribute = LoadEventInfo(type);
      cachedEventInfo[type] = eventAttribute;

      return eventAttribute;
    }

    private NativeEventAttribute LoadEventInfo(Type type)
    {
      NativeEventAttribute attribute = type.GetCustomAttribute<NativeEventAttribute>();
      if (attribute != null)
      {
        return attribute;
      }

      throw new InvalidOperationException($"Event Type {type.GetFullName()} does not define an event info attribute!");
    }

    private EventHandler CreateEventHandler<TObject, TEvent>()
      where TEvent : NativeEvent<TObject, TEvent>, new()
      where TObject : NwObject
    {
      EventHandler eventHandler = new EventHandler();

      scriptToEventMap[eventHandler.ScriptName] = eventHandler;
      typeToHandlerMap[typeof(TEvent)] = eventHandler;

      return eventHandler;
    }

    private void InitObjectHook<TObject, TEvent>(EventHandler eventHandler, TObject nwObject, EventScriptType eventScriptType)
      where TEvent : NativeEvent<TObject, TEvent>, new()
      where TObject : NwObject
    {
      string existingScript = NWScript.GetEventScript(nwObject, (int) eventScriptType);
      if (existingScript == eventHandler.ScriptName)
      {
        return;
      }

      Log.Debug($"Hooking native script event \"{eventScriptType}\" on object \"{nwObject.Name}\". Previous script: \"{existingScript}\"");

      NWScript.SetEventScript(nwObject, (int) eventScriptType, eventHandler.ScriptName);
      if (!string.IsNullOrEmpty(existingScript))
      {
        eventHandler.Subscribe<TObject, TEvent>(nwObject, gameEvent => ContinueWithNative(existingScript, nwObject));
      }
    }

    private void ContinueWithNative(string scriptName, NwObject objSelf)
    {
      NativeScript.Execute(scriptName, objSelf);
    }

    private void RemoveHandler(Type type, string scriptName)
    {
      typeToHandlerMap.Remove(type);
      scriptToEventMap.Remove(scriptName);
    }
  }
}
