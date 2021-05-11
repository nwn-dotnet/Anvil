using System;
using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnStoreRequestSell : IEvent
  {
    public bool PreventSell { get; set; }

    public Lazy<bool> Result { get; private set; }

    public NwCreature Creature { get; private init; }

    public NwItem Item { get; private init; }

    public NwStore Store { get; private init; }

    public int Price { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.RequestSellHook>
    {
      internal delegate int RequestSellHook(void* pCreature, uint oidItemToBuy, uint oidStore);

      protected override FunctionHook<RequestSellHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, uint, int> pHook = &OnRequestSell;
        return HookService.RequestHook<RequestSellHook>(pHook, FunctionsLinux._ZN12CNWSCreature11RequestSellEjj, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnRequestSell(void* pCreature, uint oidItemToSell, uint oidStore)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);
        NwItem item = oidItemToSell.ToNwObject<NwItem>();
        NwStore store = oidStore.ToNwObject<NwStore>();

        int price = 0;

        if (store != null && item != null)
        {
          price = store.Store.CalculateItemBuyPrice(item, creature.m_idSelf);
        }

        OnStoreRequestSell eventData = new OnStoreRequestSell
        {
          Creature = creature.m_idSelf.ToNwObject<NwCreature>(),
          Item = item,
          Store = store,
          Price = price
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventSell && Hook.CallOriginal(pCreature, oidItemToSell, oidStore).ToBool());
        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
      }
    }
  }
}
