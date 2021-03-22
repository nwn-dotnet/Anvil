using System;
using System.Collections.Generic;
using NLog;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  [ServiceBinding(typeof(EventService))]
  [BindingOrder(BindingOrder.API)]
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
      where TEvent : IEvent, new()
      where TEventFactory : IEventFactory
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(true);
      eventHandler.Subscribe(nwObject, handler);

      return GetEventFactory<TEventFactory>();
    }

    public TEventFactory SubscribeAll<TEvent, TEventFactory>(Action<TEvent> handler)
      where TEvent : IEvent, new()
      where TEventFactory : IEventFactory
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(true);
      eventHandler.SubscribeAll(handler);

      return GetEventFactory<TEventFactory>();
    }

    public void Unsubscribe<TEvent, TEventFactory>(NwObject nwObject, Action<TEvent> handler)
      where TEvent : IEvent, new()
      where TEventFactory : IEventFactory, new()
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(false);

      if (eventHandler == null)
      {
        return;
      }

      eventHandler.Unsubscribe(nwObject, handler);

      if (!eventHandler.HasSubscribers)
      {
        TEventFactory eventFactory = GetEventFactory<TEventFactory>();
        eventFactory.Unregister<TEvent>();
        eventHandlers.Remove(typeof(TEvent));
      }
    }

    public void UnsubscribeAll<TEvent, TEventFactory>(Action<TEvent> handler)
      where TEvent : IEvent, new()
      where TEventFactory : IEventFactory, new()
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(false);

      if (eventHandler == null)
      {
        return;
      }

      eventHandler.UnsubscribeAll(handler);

      if (!eventHandler.HasSubscribers)
      {
        TEventFactory eventFactory = GetEventFactory<TEventFactory>();
        eventFactory.Unregister<TEvent>();
        eventHandlers.Remove(typeof(TEvent));
      }
    }

    public TEvent ProcessEvent<TEvent>(TEvent eventData) where TEvent : IEvent
    {
      if (!eventHandlers.TryGetValue(eventData.GetType(), out EventHandler handler))
      {
        return eventData;
      }

      handler.ProcessEvent(eventData);
      return eventData;
    }

    public TEventFactory GetEventFactory<TEventFactory>() where TEventFactory : IEventFactory
    {
      if (!eventFactories.TryGetValue(typeof(TEventFactory), out IEventFactory factory))
      {
        Log.Error($"Cannot find event factory of type {typeof(TEventFactory).GetFullName()}. Are you missing a ServiceBinding?");
        return default;
      }

      if (activeFactories.Add(factory))
      {
        factory.Init(this);
      }

      return (TEventFactory)factory;
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
