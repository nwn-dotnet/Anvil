using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a player performs a party-related action (invite, kick, transfer leadership, etc.).
  /// </summary>
  public sealed class OnPartyEvent : IEvent
  {
    /// <summary>
    /// Gets the party event type.
    /// </summary>
    public PartyEventType EventType { get; private init; }

    /// <summary>
    /// Gets the player initiating the party action.
    /// </summary>
    public NwPlayer Player { get; private init; } = null!;

    /// <summary>
    /// Set to true to prevent the party action.
    /// </summary>
    public bool PreventEvent { get; set; }

    /// <summary>
    /// Gets the result of the party action.
    /// </summary>
    public Lazy<bool> Result { get; private set; } = null!;

    /// <summary>
    /// Gets the target creature of the party action, if applicable.
    /// </summary>
    public NwCreature Target { get; private init; } = null!;

    NwObject? IEvent.Context => Player.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.HandlePlayerToServerParty> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, byte, int> pHook = &OnHandlePartyMessage;
        Hook = HookService.RequestHook<Functions.CNWSMessage.HandlePlayerToServerParty>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnHandlePartyMessage(void* pMessage, void* pPlayer, byte nMinor)
      {
        PartyEventType eventType = (PartyEventType)nMinor;

        if (!Enum.IsDefined(eventType))
        {
          return Hook.CallOriginal(pMessage, pPlayer, nMinor);
        }

        CNWSMessage message = CNWSMessage.FromPointer(pMessage);
        uint oidTarget = message.PeekMessage<uint>(0) & 0x7FFFFFFF;

        OnPartyEvent eventData = new OnPartyEvent
        {
          EventType = eventType,
          Player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          Target = oidTarget.ToNwObject<NwCreature>()!,
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventEvent && Hook.CallOriginal(pMessage, pPlayer, nMinor).ToBool());
        ProcessEvent(EventCallbackType.Before, eventData);

        if (eventData.PreventEvent)
        {
          message.ClearReadMessage();
        }

        int retVal = eventData.Result.Value.ToInt();
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnPartyEvent"/>
    public event Action<OnPartyEvent> OnPartyEvent
    {
      add => EventService.Subscribe<OnPartyEvent, OnPartyEvent.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnPartyEvent, OnPartyEvent.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnPartyEvent"/>
    public event Action<OnPartyEvent> OnPartyEvent
    {
      add => EventService.SubscribeAll<OnPartyEvent, OnPartyEvent.Factory>(value);
      remove => EventService.UnsubscribeAll<OnPartyEvent, OnPartyEvent.Factory>(value);
    }
  }
}
