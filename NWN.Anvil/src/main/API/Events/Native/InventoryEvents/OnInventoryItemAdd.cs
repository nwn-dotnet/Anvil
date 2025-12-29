using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when an item is added to an inventory of an item or placeable.
  /// </summary>
  public sealed class OnInventoryItemAdd : IEvent
  {
    /// <summary>
    /// Gets the item or placeable that is receiving the item.
    /// </summary>
    public NwGameObject AcquiredBy { get; private init; } = null!;

    /// <summary>
    /// Gets the item being added.
    /// </summary>
    public NwItem Item { get; private init; } = null!;

    /// <summary>
    /// Set to true to prevent the item from being added.
    /// </summary>
    public bool PreventItemAdd { get; set; }

    /// <summary>
    /// Gets the result of the add operation.
    /// </summary>
    public Lazy<bool> Result { get; private set; } = null!;

    NwObject IEvent.Context => AcquiredBy;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CItemRepository.AddItem> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void**, byte, byte, byte, int, int, int> pHook = &OnAddItem;
        Hook = HookService.RequestHook<Functions.CItemRepository.AddItem>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnAddItem(void* pItemRepository, void** ppItem, byte x, byte y, byte z, int bAllowEncumbrance, int bMergeItem)
      {
        CItemRepository itemRepository = CItemRepository.FromPointer(pItemRepository);
        NwGameObject? parent = itemRepository.m_oidParent.ToNwObject<NwGameObject>();

        // Early out if parent isn't an item or placeable or Bad Things(tm) happen
        if (parent is null || parent is not NwItem && parent is not NwPlaceable)
        {
          return Hook.CallOriginal(pItemRepository, ppItem, x, y, z, bAllowEncumbrance, bMergeItem);
        }

        if (ppItem == null)
        {
          return Hook.CallOriginal(pItemRepository, ppItem, x, y, z, bAllowEncumbrance, bMergeItem);
        }

        OnInventoryItemAdd eventData = new OnInventoryItemAdd
        {
          AcquiredBy = parent,
          Item = CNWSItem.FromPointer(*ppItem).ToNwObject<NwItem>()!,
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventItemAdd && Hook.CallOriginal(pItemRepository, ppItem, x, y, z, bAllowEncumbrance, bMergeItem).ToBool());
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
  public sealed partial class NwItem
  {
    /// <inheritdoc cref="Events.OnInventoryItemAdd"/>
    public event Action<OnInventoryItemAdd> OnInventoryItemAdd
    {
      add => EventService.Subscribe<OnInventoryItemAdd, OnInventoryItemAdd.Factory>(this, value);
      remove => EventService.Unsubscribe<OnInventoryItemAdd, OnInventoryItemAdd.Factory>(this, value);
    }
  }

  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="Events.OnInventoryItemAdd"/>
    public event Action<OnInventoryItemAdd> OnInventoryItemAdd
    {
      add => EventService.Subscribe<OnInventoryItemAdd, OnInventoryItemAdd.Factory>(this, value);
      remove => EventService.Unsubscribe<OnInventoryItemAdd, OnInventoryItemAdd.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnInventoryItemAdd"/>
    public event Action<OnInventoryItemAdd> OnInventoryItemAdd
    {
      add => EventService.SubscribeAll<OnInventoryItemAdd, OnInventoryItemAdd.Factory>(value);
      remove => EventService.UnsubscribeAll<OnInventoryItemAdd, OnInventoryItemAdd.Factory>(value);
    }
  }
}
