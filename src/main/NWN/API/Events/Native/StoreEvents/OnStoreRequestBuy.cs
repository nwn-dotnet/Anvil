using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnStoreRequestBuy : IEvent
  {
    public bool PreventBuy { get; set; }

    public Lazy<bool> Result { get; private set; }

    public NwCreature Creature { get; private init; }

    public NwItem Item { get; private init; }

    public NwStore Store { get; private init; }

    public int Price { get; private init; }

    NwObject IEvent.Context => Creature;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature10RequestBuyEjjj)]
    internal delegate int RequestBuyHook(IntPtr pCreature, uint oidItemToBuy, uint oidStore, uint oidDesiredRepository);

    internal class Factory : NativeEventFactory<RequestBuyHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<RequestBuyHook> RequestHook(HookService hookService)
        => hookService.RequestHook<RequestBuyHook>(OnRequestBuy, HookOrder.Early);

      private int OnRequestBuy(IntPtr pCreature, uint oidItemToBuy, uint oidStore, uint oidDesiredRepository)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);
        NwItem item = oidItemToBuy.ToNwObject<NwItem>();
        NwStore store = oidStore.ToNwObject<NwStore>();

        int price = 0;

        if (store != null && item != null)
        {
          price = store.Store.CalculateItemSellPrice(item, creature.m_idSelf);
        }

        OnStoreRequestBuy eventData = new OnStoreRequestBuy
        {
          Creature = creature.m_idSelf.ToNwObject<NwCreature>(),
          Item = item,
          Store = store,
          Price = price
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventBuy && Hook.CallOriginal(pCreature, oidItemToBuy, oidStore, oidDesiredRepository).ToBool());
        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
      }
    }
  }
}
