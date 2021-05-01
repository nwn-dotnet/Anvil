using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed unsafe class OnInventoryItemAdd : IEvent
  {
    public NwGameObject AcquiredBy { get; private init; }

    public NwItem Item { get; private init; }

    public bool PreventItemAdd { get; set; }

    public Lazy<bool> Result { get; private set; }

    NwObject IEvent.Context => AcquiredBy;

    [NativeFunction(NWNXLib.Functions._ZN15CItemRepository7AddItemEPP8CNWSItemhhii)]
    internal delegate int AddItemHook(IntPtr pItemRepository, void** ppItem, byte x, byte y, byte z, int bAllowEncumbrance, int bMergeItem);

    internal class Factory : NativeEventFactory<AddItemHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<AddItemHook> RequestHook(HookService hookService)
        => hookService.RequestHook<AddItemHook>(OnAddItem, HookOrder.Early);

      private int OnAddItem(IntPtr pItemRepository, void** ppItem, byte x, byte y, byte z, int bAllowEncumbrance, int bMergeItem)
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
