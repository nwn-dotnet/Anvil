using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnBarterStart : IEvent
  {
    public NwPlayer Initiator { get; private init; } = null!;

    public NwPlayer Target { get; private init; } = null!;

    NwObject? IEvent.Context => Initiator.ControlledCreature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<StartBarterHook> Hook { get; set; } = null!;

      private delegate void StartBarterHook(void* pMessage, void* pPlayer);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void> pHook = &OnStartBarter;
        Hook = HookService.RequestHook<StartBarterHook>(pHook, FunctionsLinux._ZN11CNWSMessage38HandlePlayerToServerBarter_StartBarterEP10CNWSPlayer, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnStartBarter(void* pMessage, void* pPlayer)
      {
        CNWSMessage message = CNWSMessage.FromPointer(pMessage);

        OnBarterStart eventData = ProcessEvent(EventCallbackType.Before, new OnBarterStart
        {
          Initiator = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          Target = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwPlayer()!,
        });

        Hook.CallOriginal(pMessage, pPlayer);
        ProcessEvent(EventCallbackType.After, eventData);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnBarterStart"/>
    public event Action<OnBarterStart> OnBarterStart
    {
      add => EventService.Subscribe<OnBarterStart, OnBarterStart.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnBarterStart, OnBarterStart.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnBarterStart"/>
    public event Action<OnBarterStart> OnBarterStart
    {
      add => EventService.SubscribeAll<OnBarterStart, OnBarterStart.Factory>(value);
      remove => EventService.UnsubscribeAll<OnBarterStart, OnBarterStart.Factory>(value);
    }
  }
}
