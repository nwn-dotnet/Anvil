using System;
using System.Runtime.InteropServices;
using NWN.API.Events;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnPartyEvent : IEvent
  {
    public bool PreventEvent { get; set; }

    public Lazy<bool> Result { get; private set; }

    public PartyEventType EventType { get; private init; }

    public NwPlayer Player { get; private init; }

    public NwCreature Target { get; private init; }

    NwObject IEvent.Context
    {
      get => Player.ControlledCreature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.HandlePartyMessageHook>
    {
      internal delegate int HandlePartyMessageHook(void* pMessage, void* pPlayer, byte nMinor);

      protected override FunctionHook<HandlePartyMessageHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, byte, int> pHook = &OnHandlePartyMessage;
        return HookService.RequestHook<HandlePartyMessageHook>(pHook, FunctionsLinux._ZN11CNWSMessage25HandlePlayerToServerPartyEP10CNWSPlayerh, HookOrder.Early);
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
          Player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer(),
          Target = oidTarget.ToNwObject<NwCreature>(),
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventEvent && Hook.CallOriginal(pMessage, pPlayer, nMinor).ToBool());
        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnPartyEvent"/>
    public event Action<OnPartyEvent> OnPartyEvent
    {
      add => EventService.Subscribe<OnPartyEvent, OnPartyEvent.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnPartyEvent, OnPartyEvent.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {

  }
}
