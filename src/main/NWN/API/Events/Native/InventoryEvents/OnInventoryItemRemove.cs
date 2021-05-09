using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnInventoryItemRemove : IEvent
  {
    public NwGameObject RemovedFrom { get; private init; }

    public NwItem Item { get; private init; }

    NwObject IEvent.Context => RemovedFrom;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.RemoveItemHook>
    {
      internal delegate int RemoveItemHook(void* pItemRepository, void* pItem);

      protected override FunctionHook<RemoveItemHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, int> pHook = &OnRemoveItem;
        return HookService.RequestHook<RemoveItemHook>(pHook, NWNXLib.Functions._ZN15CItemRepository10RemoveItemEP8CNWSItem, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static int OnRemoveItem(void* pItemRepository, void* pItem)
      {
        CItemRepository itemRepository = new CItemRepository(pItemRepository, false);
        NwGameObject parent = itemRepository.m_oidParent.ToNwObject<NwGameObject>();

        // Early out if parent isn't an item or placeable or Bad Things(tm) happen
        if (parent is null || parent is not NwItem && parent is not NwPlaceable)
        {
          return Hook.CallOriginal(pItemRepository, pItem);
        }

        ProcessEvent(new OnInventoryItemRemove
        {
          RemovedFrom = parent,
          Item = new NwItem(new CNWSItem(pItem, false)),
        });

        return Hook.CallOriginal(pItemRepository, pItem);
      }
    }
  }
}
