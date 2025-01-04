using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.Services;
using NLog;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Event factory for built-in game events.
  /// </summary>
  [ServiceBinding(typeof(IEventFactory))]
  [ServiceBinding(typeof(IScriptDispatcher))]
  public sealed partial class GameEventFactory(Lazy<EventService> eventService, VirtualMachine virtualMachine) : IEventFactory<GameEventFactory.RegistrationData>, IScriptDispatcher, IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // Caches
    private readonly Dictionary<EventScriptType, Func<IEvent>> eventConstructorCache = new Dictionary<EventScriptType, Func<IEvent>>();
    private readonly Dictionary<Type, GameEventAttribute> eventInfoCache = new Dictionary<Type, GameEventAttribute>();
    private readonly Dictionary<EventKey, string> originalCallLookup = new Dictionary<EventKey, string>();

    public int ExecutionOrder => -10000;

    public void Register<TEvent>(RegistrationData data) where TEvent : IEvent, new()
    {
      EventScriptType eventScriptType = GetEventInfo(typeof(TEvent)).EventScriptType;

      CheckConstructorRegistered<TEvent>(eventScriptType);
      UpdateEventScript(data.NwObject, eventScriptType, data.CallOriginal);
    }

    public void Unregister<TEvent>() where TEvent : IEvent, new() {}

    ScriptHandleResult IScriptDispatcher.ExecuteScript(string? scriptName, uint oidSelf)
    {
      if (scriptName != ScriptConstants.GameEventScriptName)
      {
        return ScriptHandleResult.NotHandled;
      }

      EventScriptType eventScriptType = (EventScriptType)NWScript.GetCurrentlyRunningEvent(false.ToInt());
      if (eventScriptType == EventScriptType.None)
      {
        return ScriptHandleResult.NotHandled;
      }

      if (eventConstructorCache.TryGetValue(eventScriptType, out Func<IEvent>? value))
      {
        if (originalCallLookup.TryGetValue(new EventKey(eventScriptType, oidSelf), out scriptName))
        {
          virtualMachine.Execute(scriptName, oidSelf, eventScriptType);
        }

        eventService.Value.ProcessEvent(EventCallbackType.Before, value.Invoke());
        return ScriptHandleResult.Handled;
      }

      return ScriptHandleResult.NotHandled;
    }

    private void CheckConstructorRegistered<TEvent>(EventScriptType eventScriptType) where TEvent : IEvent, new()
    {
      if (eventConstructorCache.ContainsKey(eventScriptType))
      {
        return;
      }

      static IEvent Constructor()
      {
        return new TEvent();
      }

      eventConstructorCache[eventScriptType] = Constructor;
    }

    private GameEventAttribute GetEventInfo(Type type)
    {
      if (eventInfoCache.TryGetValue(type, out GameEventAttribute? eventAttribute))
      {
        return eventAttribute;
      }

      eventAttribute = LoadEventInfo(type);
      eventInfoCache[type] = eventAttribute;

      return eventAttribute;
    }

    private GameEventAttribute LoadEventInfo(Type type)
    {
      GameEventAttribute? attribute = type.GetCustomAttribute<GameEventAttribute>();
      if (attribute != null)
      {
        return attribute;
      }

      throw new InvalidOperationException($"Event Type {type.GetFullName()} does not define an event info attribute!");
    }

    private void UpdateEventScript(NwObject nwObject, EventScriptType eventType, bool callOriginal)
    {
      string existingScript = NWScript.GetEventScript(nwObject, (int)eventType);
      if (existingScript == ScriptConstants.GameEventScriptName)
      {
        return;
      }

      Log.Debug("Hooking native script event {EventType} on object {Object}. Previous script: {ExistingScript}", eventType, nwObject.Name, existingScript);
      NWScript.SetEventScript(nwObject, (int)eventType, ScriptConstants.GameEventScriptName);

      if (callOriginal && !string.IsNullOrWhiteSpace(existingScript))
      {
        originalCallLookup[new EventKey(eventType, nwObject)] = existingScript;
      }
    }

    public void Dispose()
    {
      // Restore the original event scripts for subscribed objects.
      foreach ((EventKey key, string script) in originalCallLookup)
      {
        NWScript.SetEventScript(key.GameObject, (int)key.EventScriptType, script);
      }
    }
  }
}
