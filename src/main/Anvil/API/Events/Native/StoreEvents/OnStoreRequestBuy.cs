using System;
using System.Runtime.InteropServices;
using NWN.API.Events;
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

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.RequestBuyHook>
    {
      internal delegate int RequestBuyHook(void* pCreature, uint oidItemToBuy, uint oidStore, uint oidDesiredRepository);

      protected override FunctionHook<RequestBuyHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, uint, uint, int> pHook = &OnRequestBuy;
        return HookService.RequestHook<RequestBuyHook>(pHook, FunctionsLinux._ZN12CNWSCreature10RequestBuyEjjj, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnRequestBuy(void* pCreature, uint oidItemToBuy, uint oidStore, uint oidDesiredRepository)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
        NwItem item = oidItemToBuy.ToNwObject<NwItem>();
        NwStore store = oidStore.ToNwObject<NwStore>();

        int price = 0;

        if (store != null && item != null)
        {
          price = store.Store.CalculateItemSellPrice(item, creature.m_idSelf);
        }

        OnStoreRequestBuy eventData = new OnStoreRequestBuy
        {
          Creature = creature.ToNwObject<NwCreature>(),
          Item = item,
          Store = store,
          Price = price,
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventBuy && Hook.CallOriginal(pCreature, oidItemToBuy, oidStore, oidDesiredRepository).ToBool());
        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnStoreRequestBuy"/>
    public event Action<OnStoreRequestBuy> OnStoreRequestBuy
    {
      add => EventService.Subscribe<OnStoreRequestBuy, OnStoreRequestBuy.Factory>(this, value);
      remove => EventService.Unsubscribe<OnStoreRequestBuy, OnStoreRequestBuy.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnStoreRequestBuy"/>
    public event Action<OnStoreRequestBuy> OnStoreRequestBuy
    {
      add => EventService.SubscribeAll<OnStoreRequestBuy, OnStoreRequestBuy.Factory>(value);
      remove => EventService.UnsubscribeAll<OnStoreRequestBuy, OnStoreRequestBuy.Factory>(value);
    }
  }
}
