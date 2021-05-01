using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnInventoryItemRemove : IEvent
  {
    public NwGameObject RemovedFrom { get; private init; }

    public NwItem Item { get; private init; }

    NwObject IEvent.Context => RemovedFrom;

    [NativeFunction(NWNXLib.Functions._ZN15CItemRepository10RemoveItemEP8CNWSItem)]
    internal delegate int RemoveItemHook(IntPtr pItemRepository, IntPtr pItem);

    internal class Factory : NativeEventFactory<RemoveItemHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<RemoveItemHook> RequestHook(HookService hookService)
        => hookService.RequestHook<RemoveItemHook>(OnRemoveItem, HookOrder.Earliest);

      private int OnRemoveItem(IntPtr pItemRepository, IntPtr pItem)
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
