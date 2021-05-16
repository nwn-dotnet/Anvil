using System;
using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnInventoryItemAdd : IEvent
  {
    public NwGameObject AcquiredBy { get; private init; }

    public NwItem Item { get; private init; }

    public bool PreventItemAdd { get; set; }

    public Lazy<bool> Result { get; private set; }

    NwObject IEvent.Context => AcquiredBy;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.AddItemHook>
    {
      internal delegate int AddItemHook(void* pItemRepository, void** ppItem, byte x, byte y, byte z, int bAllowEncumbrance, int bMergeItem);

      protected override FunctionHook<AddItemHook> RequestHook()
      {
        delegate* unmanaged<void*, void**, byte, byte, byte, int, int, int> pHook = &OnAddItem;
        return HookService.RequestHook<AddItemHook>(pHook, FunctionsLinux._ZN15CItemRepository7AddItemEPP8CNWSItemhhii, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnAddItem(void* pItemRepository, void** ppItem, byte x, byte y, byte z, int bAllowEncumbrance, int bMergeItem)
      {
        CItemRepository itemRepository = new CItemRepository(pItemRepository, false);
        NwGameObject parent = itemRepository.m_oidParent.ToNwObject<NwGameObject>();

        // Early out if parent isn't an item or placeable or Bad Things(tm) happen
        if (parent is null || parent is not NwItem && parent is not NwPlaceable)
        {
          return Hook.CallOriginal(pItemRepository, ppItem, x, y, z, bAllowEncumbrance, bMergeItem);
        }

        OnInventoryItemAdd eventData = new OnInventoryItemAdd
        {
          AcquiredBy = parent,
          Item = ppItem == null || *ppItem == null ? null : new NwItem(new CNWSItem(*ppItem, false)),
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventItemAdd && Hook.CallOriginal(pItemRepository, ppItem, x, y, z, bAllowEncumbrance, bMergeItem).ToBool());
        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
      }
    }
  }
}
