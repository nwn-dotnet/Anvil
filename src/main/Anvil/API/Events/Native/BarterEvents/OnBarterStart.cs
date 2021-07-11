using System;
using System.Runtime.InteropServices;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;

namespace NWN.API.Events
{
  public sealed class OnBarterStart : IEvent
  {
    public NwPlayer Initiator { get; private init; }

    public NwPlayer Target { get; private init; }

    NwObject IEvent.Context
    {
      get => Initiator.ControlledCreature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.StartBarterHook>
    {
      internal delegate void StartBarterHook(void* pMessage, void* pPlayer);

      protected override FunctionHook<StartBarterHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void> pHook = &OnStartBarter;
        return HookService.RequestHook<StartBarterHook>(pHook, FunctionsLinux._ZN11CNWSMessage38HandlePlayerToServerBarter_StartBarterEP10CNWSPlayer, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnStartBarter(void* pMessage, void* pPlayer)
      {
        CNWSMessage message = CNWSMessage.FromPointer(pMessage);

        ProcessEvent(new OnBarterStart
        {
          Initiator = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer(),
          Target = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwPlayer(),
        });

        Hook.CallOriginal(pMessage, pPlayer);
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnBarterStart"/>
    public event Action<OnBarterStart> OnBarterStart
    {
      add => EventService.Subscribe<OnBarterStart, OnBarterStart.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnBarterStart, OnBarterStart.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnBarterStart"/>
    public event Action<OnBarterStart> OnBarterStart
    {
      add => EventService.SubscribeAll<OnBarterStart, OnBarterStart.Factory>(value);
      remove => EventService.UnsubscribeAll<OnBarterStart, OnBarterStart.Factory>(value);
    }
  }
}
