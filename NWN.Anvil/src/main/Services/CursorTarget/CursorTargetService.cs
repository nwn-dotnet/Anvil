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
    /// <param name="settings">Display and behaviour options for the target mode.</param>
    internal void EnterTargetMode(NwPlayer player, Action<ModuleEvents.OnPlayerTarget> handler, TargetModeSettings? settings = default)
    {
      UnregisterHandlerForPlayer(player, true);
      RegisterHandlerForPlayer(player, handler);
      settings ??= new TargetModeSettings();

      if (settings.TargetingData != null)
      {
        TargetingData data = settings.TargetingData;
        NWScript.SetEnterTargetingModeData(player.ControlledCreature, (int)data.Shape, data.Size.X, data.Size.Y, (int)data.Flags, data.Range, data.Spell?.Id ?? -1, data.Feat?.Id ?? -1);
      }

      NWScript.EnterTargetingMode(player.ControlledCreature, (int)settings.ValidTargets, (int)settings.CursorType, (int)settings.BadCursorType);
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
