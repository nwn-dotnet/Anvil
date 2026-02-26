using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnBarterBarterAddItem : IEvent
  {
    public NwPlayer BarterTarget { get; private init; } = null!;
    public NwPlayer Initiator { get; private init; } = null!;

    public NwItem Item { get; private init; } = null!;

    /// <summary>
    /// Gets or sets if the AddItem trigger event should be skipped.
    /// </summary>
    public bool Skip { get; set; }

    NwObject IEvent.Context => Item;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSBarter.BarterAddItem> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void**, uint> pHook = &OnBarterBarterAddItem;
        Hook = HookService.RequestHook<Functions.CNWSBarter.BarterAddItem>(pHook, HookOrder.Early);
        return [Hook];

      }

      [UnmanagedCallersOnly]
      private static void OnBarterBarterAddItem(void* pInitiator, void* pTargetPlayer, uint oidItem)
      {
        OnBarterBarterAddItem eventData = new OnBarterBarterAddItem
        {
          Initiator = CNWSPlayer.FromPointer(pInitiator).ToNwPlayer()!,
          BarterTarget = CNWSPlayer.FromPointer(pTargetPlayer).ToNwPlayer()!,
          Item = oidItem.ToNwObject<NwItem>()!,
        };

        if (!eventData.Skip)
        {
          Hook.CallOriginal(pInitiator, pTargetPlayer, oidItem);
        }
        else
        {
          ProcessEvent(EventCallbackType.After, eventData);
        }
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnBarterBarterAddItem"/>
    public event Action<OnBarterBarterAddItem> OnBarterBarterAddItem
    {
      add => EventService.Subscribe<OnBarterBarterAddItem, OnBarterBarterAddItem.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnBarterBarterAddItem, OnBarterBarterAddItem.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnBarterBarterAddItem"/>
    public event Action<OnBarterBarterAddItem> OnBarterBarterAddItem
    {
      add => EventService.SubscribeAll<OnBarterBarterAddItem, OnBarterBarterAddItem.Factory>(value);
      remove => EventService.UnsubscribeAll<OnBarterBarterAddItem, OnBarterBarterAddItem.Factory>(value);
    }
  }
}
