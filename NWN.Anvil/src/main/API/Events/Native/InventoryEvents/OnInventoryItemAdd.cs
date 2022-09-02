using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnInventoryItemAdd : IEvent
  {
    public NwGameObject AcquiredBy { get; private init; } = null!;

    public NwItem Item { get; private init; } = null!;

    public bool PreventItemAdd { get; set; }

    public Lazy<bool> Result { get; private set; } = null!;

    NwObject IEvent.Context => AcquiredBy;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<AddItemHook> Hook { get; set; } = null!;

      private delegate int AddItemHook(void* pItemRepository, void** ppItem, byte x, byte y, byte z, int bAllowEncumbrance, int bMergeItem);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void**, byte, byte, byte, int, int, int> pHook = &OnAddItem;
        Hook = HookService.RequestHook<AddItemHook>(pHook, FunctionsLinux._ZN15CItemRepository7AddItemEPP8CNWSItemhhii, HookOrder.Early);
        return new IDisposable[] { Hook };
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
