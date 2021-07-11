using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.API.Events;
using NLog;

namespace Anvil.Services
{
  [ServiceBinding(typeof(EventService))]
  [ServiceBindingOptions(BindingOrder.API)]
  public sealed partial class EventService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly Dictionary<Type, IEventFactory> eventFactories = new Dictionary<Type, IEventFactory>();
    private readonly Dictionary<Type, EventHandler> eventHandlers = new Dictionary<Type, EventHandler>();

    public EventService(IEnumerable<IEventFactory> eventFactories)
    {
      foreach (IEventFactory eventFactory in eventFactories)
      {
        this.eventFactories[eventFactory.GetType()] = eventFactory;
      }
    }

    public void Subscribe<TEvent, TFactory>(NwObject nwObject, Action<TEvent> handler)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory<NullRegistrationData>
    {
      Subscribe<TEvent, TFactory, NullRegistrationData>(nwObject, default, handler);
    }

    public void Subscribe<TEvent, TFactory, TRegData>(NwObject nwObject, TRegData registrationData, Action<TEvent> handler)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory<TRegData>
    {
      if (ReferenceEquals(nwObject, null))
      {
        return;
      }

      AddObjectHandler(nwObject, handler);
      TFactory factory = GetEventFactory<TFactory>();
      factory.Register<TEvent>(registrationData);
    }

    public void SubscribeAll<TEvent, TFactory>(Action<TEvent> handler)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory<NullRegistrationData>
    {
      SubscribeAll<TEvent, TFactory, NullRegistrationData>(default, handler);
    }

    public void SubscribeAll<TEvent, TFactory, TRegData>(TRegData registrationData, Action<TEvent> handler)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory<TRegData>
    {
      AddGlobalHandler(handler);
      TFactory factory = GetEventFactory<TFactory>();
      factory.Register<TEvent>(registrationData);
    }

    public void Unsubscribe<TEvent, TFactory>(NwObject nwObject, Action<TEvent> handler)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory
    {
      if (ReferenceEquals(nwObject, null))
      {
        return;
      }

      RemoveObjectHandler(nwObject, handler);
      TryCleanupHandler<TEvent, TFactory>();
    }

    public void UnsubscribeAll<TEvent, TFactory>(Action<TEvent> handler)
      where TEvent : IEvent, new()
      where TFactory : IEventFactory
    {
      RemoveGlobalHandler(handler);
      TryCleanupHandler<TEvent, TFactory>();
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

    private void TryCleanupHandler<TEvent, TFactory>()
      where TEvent : IEvent, new()
      where TFactory : IEventFactory
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

      TFactory factory = GetEventFactory<TFactory>();
      factory.Unregister<TEvent>();
    }

    private TFactory GetEventFactory<TFactory>() where TFactory : IEventFactory
    {
      Type factoryType = typeof(TFactory);

      if (!eventFactories.TryGetValue(factoryType, out IEventFactory factory))
      {
        Log.Error($"Cannot find event factory of type {factoryType.GetFullName()}. Are you missing a ServiceBinding?");
        return default;
      }

      return (TFactory)factory;
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
