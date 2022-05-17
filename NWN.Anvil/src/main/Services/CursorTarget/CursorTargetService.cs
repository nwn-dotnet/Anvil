using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.Services
{
  /// <summary>
  /// A managed implementation of selection/target mode logic utilising C# style callbacks.
  /// </summary>
  [ServiceBinding(typeof(CursorTargetService))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  internal sealed class CursorTargetService
  {
    private readonly Dictionary<NwPlayer, Action<ModuleEvents.OnPlayerTarget>> activeHandlers = new Dictionary<NwPlayer, Action<ModuleEvents.OnPlayerTarget>>();

    public CursorTargetService(EventService eventService)
    {
      eventService.SubscribeAll<ModuleEvents.OnClientLeave, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(NwModule.Instance), OnClientLeave);
    }

    /// <summary>
    /// Instructs the specified player to enter cursor targeting mode, invoking the specified handler once the player selects something.
    /// </summary>
    /// <param name="player">The player who should enter selection mode.</param>
    /// <param name="handler">The lamda/method to invoke once this player selects something.</param>
    /// <param name="validTargets">The type of objects that are valid for selection. ObjectTypes is a flags enum, so multiple types may be specified using the OR operator (ObjectTypes.Creature | ObjectTypes.Placeable).</param>
    /// <param name="cursorType">The type of cursor to show if the player is hovering over a valid target.</param>
    /// <param name="badTargetCursor">The type of cursor to show if the player is hovering over an invalid target.</param>
    internal void EnterTargetMode(NwPlayer player, Action<ModuleEvents.OnPlayerTarget> handler, ObjectTypes validTargets = ObjectTypes.All, MouseCursor cursorType = MouseCursor.Magic, MouseCursor badTargetCursor = MouseCursor.NoMagic)
    {
      UnregisterHandlerForPlayer(player, true);
      RegisterHandlerForPlayer(player, handler);
      NWScript.EnterTargetingMode(player.ControlledCreature, (int)validTargets, (int)cursorType, (int)badTargetCursor);
    }

    internal bool IsInTargetMode(NwPlayer player)
    {
      return activeHandlers.ContainsKey(player);
    }

    private ModuleEvents.OnPlayerTarget CreateCancelledEventData(NwPlayer player)
    {
      return new ModuleEvents.OnPlayerTarget
      {
        Player = player,
        TargetObject = null,
        TargetPosition = default,
      };
    }

    private void OnClientLeave(ModuleEvents.OnClientLeave eventData)
    {
      UnregisterHandlerForPlayer(eventData.Player, true);
    }

    private void RegisterHandlerForPlayer(NwPlayer player, Action<ModuleEvents.OnPlayerTarget> handler)
    {
      void InvokeEventHandlerOnce(ModuleEvents.OnPlayerTarget eventData)
      {
        UnregisterHandlerForPlayer(eventData.Player, false);
        handler.Invoke(eventData);
      }

      Action<ModuleEvents.OnPlayerTarget> eventCallback = InvokeEventHandlerOnce;
      activeHandlers.Add(player, eventCallback);

      player.OnPlayerTarget += eventCallback;
    }

    private void UnregisterHandlerForPlayer(NwPlayer player, bool cancelled)
    {
      if (activeHandlers.Remove(player, out Action<ModuleEvents.OnPlayerTarget>? eventCallback))
      {
        player.OnPlayerTarget -= eventCallback;
        if (cancelled)
        {
          eventCallback(CreateCancelledEventData(player));
        }
      }
    }
  }
}
