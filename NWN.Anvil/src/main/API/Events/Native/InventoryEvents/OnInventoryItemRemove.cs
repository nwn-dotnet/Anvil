using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when an item is removed from an inventory of an item or placeable.
  /// </summary>
  public sealed class OnInventoryItemRemove : IEvent
  {
    /// <summary>
    /// Gets the item being removed.
    /// </summary>
    public NwItem Item { get; private init; } = null!;

    /// <summary>
    /// Gets the item or placeable the item is being removed from.
    /// </summary>
    public NwGameObject RemovedFrom { get; private init; } = null!;

    NwObject IEvent.Context => RemovedFrom;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CItemRepository.RemoveItem> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int> pHook = &OnRemoveItem;
        Hook = HookService.RequestHook<Functions.CItemRepository.RemoveItem>(pHook, HookOrder.Earliest);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnRemoveItem(void* pItemRepository, void* pItem)
      {
        CItemRepository itemRepository = CItemRepository.FromPointer(pItemRepository);
        NwGameObject? parent = itemRepository.m_oidParent.ToNwObject<NwGameObject>();

        // Early out if parent isn't an item or placeable or Bad Things(tm) happen
        if (parent is null || parent is not NwItem && parent is not NwPlaceable)
        {
          return Hook.CallOriginal(pItemRepository, pItem);
        }

        OnInventoryItemRemove eventData = ProcessEvent(EventCallbackType.Before, new OnInventoryItemRemove
        {
          RemovedFrom = parent,
          Item = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>()!,
        });

        int retVal = Hook.CallOriginal(pItemRepository, pItem);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwItem
  {
    /// <inheritdoc cref="Events.OnInventoryItemRemove"/>
    public event Action<OnInventoryItemRemove> OnInventoryItemRemove
    {
      add => EventService.Subscribe<OnInventoryItemRemove, OnInventoryItemRemove.Factory>(this, value);
      remove => EventService.Unsubscribe<OnInventoryItemRemove, OnInventoryItemRemove.Factory>(this, value);
    }
  }

  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="Events.OnInventoryItemRemove"/>
    public event Action<OnInventoryItemRemove> OnInventoryItemRemove
    {
      add => EventService.Subscribe<OnInventoryItemRemove, OnInventoryItemRemove.Factory>(this, value);
      remove => EventService.Unsubscribe<OnInventoryItemRemove, OnInventoryItemRemove.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnInventoryItemRemove"/>
    public event Action<OnInventoryItemRemove> OnInventoryItemRemove
    {
      add => EventService.SubscribeAll<OnInventoryItemRemove, OnInventoryItemRemove.Factory>(value);
      remove => EventService.UnsubscribeAll<OnInventoryItemRemove, OnInventoryItemRemove.Factory>(value);
    }
  }
}
