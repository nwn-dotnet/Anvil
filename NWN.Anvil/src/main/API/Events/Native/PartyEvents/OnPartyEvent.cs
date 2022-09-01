using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnPartyEvent : IEvent
  {
    public PartyEventType EventType { get; private init; }

    public NwPlayer Player { get; private init; } = null!;
    public bool PreventEvent { get; set; }

    public Lazy<bool> Result { get; private set; } = null!;

    public NwCreature Target { get; private init; } = null!;

    NwObject? IEvent.Context => Player.ControlledCreature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<HandlePartyMessageHook> Hook { get; set; } = null!;

      private delegate int HandlePartyMessageHook(void* pMessage, void* pPlayer, byte nMinor);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, byte, int> pHook = &OnHandlePartyMessage;
        Hook = HookService.RequestHook<HandlePartyMessageHook>(pHook, FunctionsLinux._ZN11CNWSMessage25HandlePlayerToServerPartyEP10CNWSPlayerh, HookOrder.Early);
        return new IDisposable[] { Hook };
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
