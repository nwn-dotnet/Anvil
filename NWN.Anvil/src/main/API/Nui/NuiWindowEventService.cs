using System;
using System.Collections.Generic;
using Anvil.API.Events;
using Anvil.Services;

namespace Anvil.API
{
  [ServiceBinding(typeof(NuiWindowEventService))]
  internal sealed class NuiWindowEventService
  {
    private readonly Dictionary<NwPlayer, Dictionary<int, Action<ModuleEvents.OnNuiEvent>>> eventHandlers = new Dictionary<NwPlayer, Dictionary<int, Action<ModuleEvents.OnNuiEvent>>>();

    public NuiWindowEventService()
    {
      NwModule.Instance.OnNuiEvent += OnNuiEvent;
      NwModule.Instance.OnClientLeave += OnClientLeave;
    }

    public void Subscribe(NuiWindowToken token, Action<ModuleEvents.OnNuiEvent> handler)
    {
      if (!eventHandlers.TryGetValue(token.Player, out Dictionary<int, Action<ModuleEvents.OnNuiEvent>> playerHandlers))
      {
        playerHandlers = new Dictionary<int, Action<ModuleEvents.OnNuiEvent>>();
        eventHandlers[token.Player] = playerHandlers;
      }

      if (playerHandlers.ContainsKey(token.Token))
      {
        playerHandlers[token.Token] += handler;
      }
      else
      {
        playerHandlers[token.Token] = handler;
      }
    }

    public void Unsubscribe(NuiWindowToken token, Action<ModuleEvents.OnNuiEvent> handler)
    {
      if (!eventHandlers.TryGetValue(token.Player, out Dictionary<int, Action<ModuleEvents.OnNuiEvent>> playerHandlers))
      {
        return;
      }

      if (playerHandlers.ContainsKey(token.Token))
      {
        playerHandlers[token.Token] -= handler;
        if (playerHandlers[token.Token] == null)
        {
          playerHandlers.Remove(token.Token);
        }
      }
    }

    private void OnClientLeave(ModuleEvents.OnClientLeave eventData)
    {
      eventHandlers.Remove(eventData.Player);
    }

    private void OnNuiEvent(ModuleEvents.OnNuiEvent eventData)
    {
      if (eventHandlers.TryGetValue(eventData.Player, out Dictionary<int, Action<ModuleEvents.OnNuiEvent>> playerEventHandlers))
      {
        if (playerEventHandlers.TryGetValue(eventData.WindowToken, out Action<ModuleEvents.OnNuiEvent> eventHandler))
        {
          eventHandler?.Invoke(eventData);
          if (eventData.EventType == NuiEventType.Close)
          {
            playerEventHandlers.Remove(eventData.WindowToken);
          }
        }
      }
    }
  }
}
