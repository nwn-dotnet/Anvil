using System;
using System.Collections.Generic;
using NLog;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  [ServiceBinding(typeof(EventService))]
  [BindingOrder(BindingOrder.API)]
  public class EventService
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

    public void Subscribe<TEvent, TEventFactory>(NwObject obj, Action<TEvent> handler, Action<TEventFactory> factory = null)
      where TEvent : IEvent, new()
      where TEventFactory : IEventFactory
    {
      TEventFactory eventFactory = GetEventFactory<TEventFactory>();
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(true);

      if (activeFactories.Add(eventFactory))
      {
        eventFactory.Init(this);
      }

      eventFactory.Register<TEvent>(obj);
      eventHandler.Subscribe(obj, handler);
      factory?.Invoke(eventFactory);
    }

    public void Unsubscribe<TEvent, TEventFactory>(NwObject obj, Action<TEvent> handler)
      where TEvent : IEvent, new()
      where TEventFactory : IEventFactory, new()
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(false);

      if (eventHandler == null)
      {
        return;
      }

      eventHandler.Unsubscribe(obj, handler);

      if (!eventHandler.HasSubscribers)
      {
        TEventFactory eventFactory = GetEventFactory<TEventFactory>();
        eventFactory.Unregister<TEvent>();

        eventHandlers.Remove(typeof(TEvent));
        activeFactories.Remove(eventFactory);
      }
    }

    internal TEvent ProcessEvent<TEvent>(TEvent eventData) where TEvent : IEvent
    {
      if (!eventHandlers.TryGetValue(typeof(EventHandler<TEvent>), out EventHandler handler))
      {
        return eventData;
      }

      ((EventHandler<TEvent>)handler).ProcessEvent(eventData);
      return eventData;
    }

    private TEventFactory GetEventFactory<TEventFactory>() where TEventFactory : IEventFactory
    {
      if (!eventFactories.TryGetValue(typeof(TEventFactory), out IEventFactory factory))
      {
        Log.Error($"Cannot find event factory of type {typeof(TEventFactory).GetFullName()}. Are you missing a ServiceBinding?");
      }

      return (TEventFactory)factory;
    }

    private EventHandler<TEvent> GetEventHandler<TEvent>(bool createMissing) where TEvent : IEvent
    {
      if (!eventHandlers.TryGetValue(typeof(EventHandler<TEvent>), out EventHandler handler) && createMissing)
      {
        handler = new EventHandler<TEvent>();
        eventHandlers[typeof(EventHandler<TEvent>)] = handler;
      }

      return (EventHandler<TEvent>)handler;
    }
  }
}
