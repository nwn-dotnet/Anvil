using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnInventoryItemRemove : IEvent
  {
    public NwGameObject RemovedFrom { get; private init; }

    public NwItem Item { get; private init; }

    NwObject IEvent.Context
    {
      get => RemovedFrom;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.RemoveItemHook>
    {
      internal delegate int RemoveItemHook(void* pItemRepository, void* pItem);

      protected override FunctionHook<RemoveItemHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, int> pHook = &OnRemoveItem;
        return HookService.RequestHook<RemoveItemHook>(pHook, FunctionsLinux._ZN15CItemRepository10RemoveItemEP8CNWSItem, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static int OnRemoveItem(void* pItemRepository, void* pItem)
      {
        CItemRepository itemRepository = CItemRepository.FromPointer(pItemRepository);
        NwGameObject parent = itemRepository.m_oidParent.ToNwObject<NwGameObject>();

        // Early out if parent isn't an item or placeable or Bad Things(tm) happen
        if (parent is null || parent is not NwItem && parent is not NwPlaceable)
        {
          return Hook.CallOriginal(pItemRepository, pItem);
        }

        ProcessEvent(new OnInventoryItemRemove
        {
          RemovedFrom = parent,
          Item = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>(),
        });

        return Hook.CallOriginal(pItemRepository, pItem);
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
