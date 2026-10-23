using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Plugins;
using LightInject;
using NLog;

namespace Anvil.Services
{
  [ServiceBinding(typeof(EventService))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  public sealed partial class EventService : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly IServiceManager serviceManager;
    private readonly Dictionary<Type, IEventFactory> eventFactories = new Dictionary<Type, IEventFactory>();
    private readonly Dictionary<Type, EventHandler> eventHandlers = new Dictionary<Type, EventHandler>();

    public EventService(IEnumerable<IEventFactory> eventFactories, IServiceManager serviceManager)
    {
      foreach (IEventFactory eventFactory in eventFactories)
      {
        this.eventFactories[eventFactory.GetType()] = eventFactory;
      }

      this.serviceManager = serviceManager;
      serviceManager.OnContainerPostDispose += OnContainerPostDispose;
    }

    void IDisposable.Dispose()
    {
      serviceManager.OnContainerPostDispose -= OnContainerPostDispose;
    }

    public void ClearObjectSubscriptions(NwObject? nwObject)
    {
      if (ReferenceEquals(nwObject, null))
      {
        return;
      }

      foreach (EventHandler eventHandler in eventHandlers.Values)
      {
        eventHandler.ClearObjectSubscriptions(nwObject);
      }
    }

    public TEvent ProcessEvent<TEvent>(EventCallbackType eventCallbackType, TEvent eventData)
      where TEvent : IEvent
    {
      if (!eventHandlers.TryGetValue(eventData.GetType(), out EventHandler? handler))
      {
        return eventData;
      }

      handler.ProcessEvent(eventData, eventCallbackType);
      return eventData;
    }

    public void Subscribe<TEvent, TFactory>(NwObject? nwObject, Action<TEvent> handler, EventCallbackType eventCallbackType = EventCallbackType.Before)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory<NullRegistrationData>
    {
      Subscribe<TEvent, TFactory, NullRegistrationData>(nwObject, default, handler, eventCallbackType);
    }

    public void Subscribe<TEvent, TFactory, TRegData>(NwObject? nwObject, TRegData registrationData, Action<TEvent> handler, EventCallbackType eventCallbackType = EventCallbackType.Before)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory<TRegData>
    {
      if (ReferenceEquals(nwObject, null))
      {
        return;
      }

      TFactory? factory = GetEventFactory<TFactory>();
      AddObjectHandler(nwObject, handler, eventCallbackType, factory);
      factory?.Register<TEvent>(registrationData);
    }

    public void SubscribeAll<TEvent, TFactory>(Action<TEvent> handler, EventCallbackType eventCallbackType = EventCallbackType.Before)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory<NullRegistrationData>
    {
      SubscribeAll<TEvent, TFactory, NullRegistrationData>(default, handler, eventCallbackType);
    }

    public void SubscribeAll<TEvent, TFactory, TRegData>(TRegData registrationData, Action<TEvent> handler, EventCallbackType eventCallbackType = EventCallbackType.Before)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory<TRegData>
    {
      TFactory? factory = GetEventFactory<TFactory>();
      AddGlobalHandler(handler, eventCallbackType, factory);
      factory?.Register<TEvent>(registrationData);
    }

    public void Unsubscribe<TEvent, TFactory>(NwObject? nwObject, Action<TEvent> handler, EventCallbackType eventCallbackType = EventCallbackType.Before)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory
    {
      if (ReferenceEquals(nwObject, null))
      {
        return;
      }

      RemoveObjectHandler(nwObject, handler, eventCallbackType);
      TryCleanupHandler<TEvent, TFactory>();
    }

    public void UnsubscribeAll<TEvent, TFactory>(Action<TEvent> handler, EventCallbackType eventCallbackType = EventCallbackType.Before)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory
    {
      RemoveGlobalHandler(handler, eventCallbackType);
      TryCleanupHandler<TEvent, TFactory>();
    }

    private void AddGlobalHandler<TEvent>(Action<TEvent> handler, EventCallbackType eventCallbackType, IEventFactory? factory)
      where TEvent : IEvent, new()
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(true);
      eventHandler.SubscribeAll(handler, eventCallbackType, factory);
    }

    private void AddObjectHandler<TEvent>(NwObject nwObject, Action<TEvent> handler, EventCallbackType eventCallbackType, IEventFactory? factory)
      where TEvent : IEvent, new()
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(true);
      eventHandler.Subscribe(nwObject, handler, eventCallbackType, factory);
    }

    private TFactory? GetEventFactory<TFactory>() where TFactory : IEventFactory
    {
      Type factoryType = typeof(TFactory);

      if (!eventFactories.TryGetValue(factoryType, out IEventFactory? factory))
      {
        Log.Error("Cannot find event factory of type {EventFactory}. Are you missing a ServiceBinding?", factoryType.GetFullName());
        return default;
      }

      return (TFactory)factory;
    }

    private EventHandler<TEvent>? GetEventHandler<TEvent>([DoesNotReturnIf(true)] bool createMissing) where TEvent : IEvent, new()
    {
      if (!eventHandlers.TryGetValue(typeof(TEvent), out EventHandler? handler) && createMissing)
      {
        handler = new EventHandler<TEvent>();
        eventHandlers[typeof(TEvent)] = handler;
      }

      return (EventHandler<TEvent>?)handler;
    }

    // Isolated plugins may leave subscriptions behind when unloaded. They keep the plugin loaded, and would keep invoking its old code.
    private void OnContainerPostDispose(IServiceContainer container, Plugin? plugin)
    {
      if (plugin == null)
      {
        return;
      }

      int removed = 0;
      foreach ((Type eventType, EventHandler eventHandler) in eventHandlers.ToList())
      {
        removed += eventHandler.ClearPluginSubscriptions(plugin);
        if (!eventHandler.HasSubscribers)
        {
          eventHandlers.Remove(eventType);
          eventHandler.UnregisterFactories();
        }
      }

      if (removed > 0)
      {
        Log.Debug("Removed {Count} event subscriptions left by unloaded plugin {PluginName}", removed, plugin.Name.Name);
      }
    }

    private void RemoveGlobalHandler<TEvent>(Action<TEvent> handler, EventCallbackType eventCallbackType)
      where TEvent : IEvent, new()
    {
      EventHandler<TEvent>? eventHandler = GetEventHandler<TEvent>(false);
      eventHandler?.UnsubscribeAll(handler, eventCallbackType);
    }

    private void RemoveObjectHandler<TEvent>(NwObject nwObject, Action<TEvent> handler, EventCallbackType eventCallbackType)
      where TEvent : IEvent, new()
    {
      EventHandler<TEvent>? eventHandler = GetEventHandler<TEvent>(false);
      eventHandler?.Unsubscribe(nwObject, handler, eventCallbackType);
    }

    private void TryCleanupHandler<TEvent, TFactory>()
      where TEvent : IEvent, new()
      where TFactory : IEventFactory
    {
      EventHandler<TEvent>? eventHandler = GetEventHandler<TEvent>(false);
      if (eventHandler != null)
      {
        if (eventHandler.HasSubscribers)
        {
          return;
        }

        eventHandlers.Remove(typeof(TEvent));
      }

      TFactory? factory = GetEventFactory<TFactory>();
      factory?.Unregister<TEvent>();
    }
  }
}
