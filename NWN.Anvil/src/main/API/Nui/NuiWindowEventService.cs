using System;
using System.Collections.Generic;
using Anvil.API.Events;
using Anvil.Services;

namespace Anvil.API
{
  /// <summary>
  /// Service that routes NUI window events to registered handlers and performs cleanup on window close or client leave.
  /// </summary>
  [ServiceBinding(typeof(NuiWindowEventService))]
  internal sealed class NuiWindowEventService
  {
    /// <summary>
    /// Maps players and their window tokens to subscribed event handlers.
    /// </summary>
    private readonly Dictionary<NwPlayer, Dictionary<int, Action<ModuleEvents.OnNuiEvent>>> eventHandlers = new Dictionary<NwPlayer, Dictionary<int, Action<ModuleEvents.OnNuiEvent>>>();

    /// <summary>
    /// Initializes the service and subscribes to NUI and client lifecycle events.
    /// </summary>
    /// <param name="eventService">The event service used to subscribe to module events.</param>
    public NuiWindowEventService(EventService eventService)
    {
      eventService.SubscribeAll<ModuleEvents.OnNuiEvent, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(NwModule.Instance), OnNuiEvent);
      eventService.SubscribeAll<ModuleEvents.OnClientLeave, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(NwModule.Instance), OnClientLeave);
    }

    /// <summary>
    /// Subscribes a handler to NUI events for the specified window token.
    /// </summary>
    /// <param name="token">The window token to subscribe.</param>
    /// <param name="handler">The handler invoked when an event occurs.</param>
    public void Subscribe(NuiWindowToken token, Action<ModuleEvents.OnNuiEvent> handler)
    {
      if (!eventHandlers.TryGetValue(token.Player, out Dictionary<int, Action<ModuleEvents.OnNuiEvent>>? playerHandlers))
      {
        playerHandlers = new Dictionary<int, Action<ModuleEvents.OnNuiEvent>>();
        eventHandlers[token.Player] = playerHandlers;
      }

      if (!playerHandlers.TryAdd(token.Token, handler))
      {
        playerHandlers[token.Token] += handler;
      }
    }

    /// <summary>
    /// Unsubscribes a handler from NUI events for the specified window token.
    /// </summary>
    /// <param name="token">The window token to unsubscribe.</param>
    /// <param name="handler">The handler to remove.</param>
    public void Unsubscribe(NuiWindowToken token, Action<ModuleEvents.OnNuiEvent> handler)
    {
      if (eventHandlers.TryGetValue(token.Player, out Dictionary<int, Action<ModuleEvents.OnNuiEvent>>? playerHandlers))
      {
        if (playerHandlers.TryGetValue(token.Token, out Action<ModuleEvents.OnNuiEvent>? existingHandler))
        {
          existingHandler -= handler;
          if (existingHandler == null)
          {
            playerHandlers.Remove(token.Token);
          }
        }
      }
    }

    /// <summary>
    /// Handles client leave events by removing all event subscriptions for that player.
    /// </summary>
    private void OnClientLeave(ModuleEvents.OnClientLeave eventData)
    {
      eventHandlers.Remove(eventData.Player);
    }

    /// <summary>
    /// Dispatches an incoming NUI event to the subscribed handler, and removes the subscription when the window closes.
    /// </summary>
    private void OnNuiEvent(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventHandlers.TryGetValue(eventData.Player, out Dictionary<int, Action<ModuleEvents.OnNuiEvent>>? playerEventHandlers))
      {
        if (playerEventHandlers.TryGetValue(eventData.Token.Token, out Action<ModuleEvents.OnNuiEvent>? eventHandler))
        {
          eventHandler.Invoke(eventData);
          if (eventData.EventType == NuiEventType.Close)
          {
            playerEventHandlers.Remove(eventData.Token.Token);
          }
        }
      }
    }
  }
}
