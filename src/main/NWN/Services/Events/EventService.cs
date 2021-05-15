using System;
using System.Collections.Generic;
using NLog;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  [ServiceBinding(typeof(EventService))]
  [ServiceBindingOptions(BindingOrder.API)]
  public sealed partial class EventService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly Dictionary<Type, IEventFactory> eventFactories = new Dictionary<Type, IEventFactory>();
    private readonly Dictionary<Type, EventHandler> eventHandlers = new Dictionary<Type, EventHandler>();

    private readonly HashSet<IEventFactory> activeFactories = new HashSet<IEventFactory>();

    public EventService(IEnumerable<IEventFactory> eventFactories)
    {
      foreach (IEventFactory eventFactory in eventFactories)
      {
        this.eventFactories[eventFactory.GetType()] = eventFactory;
      }
    }

    public TEventFactory Subscribe<TEvent, TEventFactory>(NwObject nwObject, Action<TEvent> handler)
      where TEvent : IEvent
      where TEventFactory : IEventFactory
    {
      if (ReferenceEquals(nwObject, null))
      {
        return default;
      }

      AddObjectHandler(nwObject, handler);
      return InitEventFactory<TEventFactory>();
    }

    public void Subscribe<TEvent>(NwObject nwObject, IEnumerable<Type> factoryTypes, Action<TEvent> handler)
      where TEvent : IEvent
    {
      if (ReferenceEquals(nwObject, null))
      {
        return;
      }

      AddObjectHandler(nwObject, handler);
      InitEventFactories(factoryTypes);
    }

    public TEventFactory SubscribeAll<TEvent, TEventFactory>(Action<TEvent> handler)
      where TEvent : IEvent
      where TEventFactory : IEventFactory
    {
      AddGlobalHandler(handler);
      return InitEventFactory<TEventFactory>();
    }

    public void SubscribeAll<TEvent>(IEnumerable<Type> factoryTypes, Action<TEvent> handler)
      where TEvent : IEvent
    {
      AddGlobalHandler(handler);
      InitEventFactories(factoryTypes);
    }

    public void Unsubscribe<TEvent, TEventFactory>(NwObject nwObject, Action<TEvent> handler)
      where TEvent : IEvent
      where TEventFactory : IEventFactory
    {
      if (ReferenceEquals(nwObject, null))
      {
        return;
      }

      RemoveObjectHandler(nwObject, handler);
      TryCleanupHandler<TEvent>(typeof(TEventFactory).Yield());
    }

    public void Unsubscribe<TEvent>(NwObject nwObject, IEnumerable<Type> factoryTypes, Action<TEvent> handler)
      where TEvent : IEvent
    {
      if (ReferenceEquals(nwObject, null))
      {
        return;
      }

      RemoveObjectHandler(nwObject, handler);
      TryCleanupHandler<TEvent>(factoryTypes);
    }

    public void UnsubscribeAll<TEvent, TEventFactory>(Action<TEvent> handler)
      where TEvent : IEvent
      where TEventFactory : IEventFactory
    {
      RemoveGlobalHandler(handler);
      TryCleanupHandler<TEvent>(typeof(TEventFactory).Yield());
    }

    public void UnsubscribeAll<TEvent>(IEnumerable<Type> factoryTypes, Action<TEvent> handler)
      where TEvent : IEvent
    {
      RemoveGlobalHandler(handler);
      TryCleanupHandler<TEvent>(factoryTypes);
    }

    public void ClearObjectSubscriptions(NwObject nwObject)
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

    public TEvent ProcessEvent<TEvent>(TEvent eventData)
      where TEvent : IEvent
    {
      if (!eventHandlers.TryGetValue(eventData.GetType(), out EventHandler handler))
      {
        return eventData;
      }

      handler.ProcessEvent(eventData);
      return eventData;
    }

    private void AddObjectHandler<TEvent>(NwObject nwObject, Action<TEvent> handler)
      where TEvent : IEvent
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(true);
      eventHandler.Subscribe(nwObject, handler);
    }

    private void AddGlobalHandler<TEvent>(Action<TEvent> handler)
      where TEvent : IEvent
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(true);
      eventHandler.SubscribeAll(handler);
    }

    private void RemoveObjectHandler<TEvent>(NwObject nwObject, Action<TEvent> handler)
      where TEvent : IEvent
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(false);
      eventHandler?.Unsubscribe(nwObject, handler);
    }

    private void RemoveGlobalHandler<TEvent>(Action<TEvent> handler)
      where TEvent : IEvent
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(false);
      eventHandler?.UnsubscribeAll(handler);
    }

    private void InitEventFactories(IEnumerable<Type> factoryTypes)
    {
      foreach (Type factoryType in factoryTypes)
      {
        InitEventFactory(factoryType);
      }
    }

    private TEventFactory InitEventFactory<TEventFactory>() where TEventFactory : IEventFactory
    {
      return (TEventFactory)InitEventFactory(typeof(TEventFactory));
    }

    private IEventFactory InitEventFactory(Type factoryType)
    {
      if (!eventFactories.TryGetValue(factoryType, out IEventFactory factory))
      {
        Log.Error($"Cannot find event factory of type {factoryType.GetFullName()}. Are you missing a ServiceBinding?");
        return default;
      }

      if (activeFactories.Add(factory))
      {
        factory.Init();
      }

      return factory;
    }

    private void TryCleanupHandler<TEvent>(IEnumerable<Type> factoryTypes) where TEvent : IEvent
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(false);
      if (eventHandler != null)
      {
        if (eventHandler.HasSubscribers)
        {
          return;
        }

        eventHandlers.Remove(typeof(TEvent));
      }

      foreach (Type eventFactory in factoryTypes)
      {
        if (eventFactories.TryGetValue(eventFactory, out IEventFactory factory) && activeFactories.Remove(factory))
        {
          factory.Unregister<TEvent>();
        }
      }
    }

    private EventHandler<TEvent> GetEventHandler<TEvent>(bool createMissing) where TEvent : IEvent
    {
      if (!eventHandlers.TryGetValue(typeof(TEvent), out EventHandler handler) && createMissing)
      {
        handler = new EventHandler<TEvent>();
        eventHandlers[typeof(TEvent)] = handler;
      }

      return (EventHandler<TEvent>)handler;
    }
  }
}
