using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnItemSplit : IEvent
  {
    /// <summary>
    /// Gets the item being split.
    /// </summary>
    public NwItem ItemToSplit { get; private init; } = null!;

    /// <summary>
    /// Gets the number of items being split from the stack.
    /// </summary>
    public int NumberToSplitOff { get; private init; }

    NwObject IEvent.Context => ItemToSplit;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSItem.SplitItem> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, void> pHook = &OnItemSplit;
        Hook = HookService.RequestHook<Functions.CNWSItem.SplitItem>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static void OnItemSplit(void* pItem, int splitOff)
      {
        OnItemSplit eventData = ProcessEvent(EventCallbackType.Before, new OnItemSplit
        {
          NumberToSplitOff = splitOff,
          ItemToSplit = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>()!,
        });

        Hook.CallOriginal(pItem, splitOff);

        ProcessEvent(EventCallbackType.After, eventData);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwItem
  {
    /// <inheritdoc cref="Events.OnItemSplit"/>
    public event Action<OnItemSplit> OnItemSplit
    {
      add => EventService.Subscribe<OnItemSplit, OnItemSplit.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemSplit, OnItemSplit.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnItemSplit"/>
    public event Action<OnItemSplit> OnItemSplit
    {
      add => EventService.SubscribeAll<OnItemSplit, OnItemSplit.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemSplit, OnItemSplit.Factory>(value);
    }
  }
}
