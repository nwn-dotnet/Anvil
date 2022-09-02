using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Anvil.API;
using Anvil.API.Events;
using NLog;

namespace Anvil.Services
{
  [ServiceBinding(typeof(EventService))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
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

      AddObjectHandler(nwObject, handler, eventCallbackType);
      TFactory? factory = GetEventFactory<TFactory>();
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
      AddGlobalHandler(handler, eventCallbackType);
      TFactory? factory = GetEventFactory<TFactory>();
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

    private void AddGlobalHandler<TEvent>(Action<TEvent> handler, EventCallbackType eventCallbackType)
      where TEvent : IEvent
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(true);
      eventHandler.SubscribeAll(handler, eventCallbackType);
    }

    private void AddObjectHandler<TEvent>(NwObject nwObject, Action<TEvent> handler, EventCallbackType eventCallbackType)
      where TEvent : IEvent
    {
      EventHandler<TEvent> eventHandler = GetEventHandler<TEvent>(true);
      eventHandler.Subscribe(nwObject, handler, eventCallbackType);
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

    private EventHandler<TEvent>? GetEventHandler<TEvent>([DoesNotReturnIf(true)] bool createMissing) where TEvent : IEvent
    {
      if (!eventHandlers.TryGetValue(typeof(TEvent), out EventHandler? handler) && createMissing)
      {
        handler = new EventHandler<TEvent>();
        eventHandlers[typeof(TEvent)] = handler;
      }

      return (EventHandler<TEvent>?)handler;
    }

    private void RemoveGlobalHandler<TEvent>(Action<TEvent> handler, EventCallbackType eventCallbackType)
      where TEvent : IEvent
    {
      EventHandler<TEvent>? eventHandler = GetEventHandler<TEvent>(false);
      eventHandler?.UnsubscribeAll(handler, eventCallbackType);
    }

    private void RemoveObjectHandler<TEvent>(NwObject nwObject, Action<TEvent> handler, EventCallbackType eventCallbackType)
      where TEvent : IEvent
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
