using System;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;

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

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.RequestSellHook>
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
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
        NwItem item = oidItemToSell.ToNwObject<NwItem>();
        NwStore store = oidStore.ToNwObject<NwStore>();

        int price = 0;

        if (store != null && item != null)
        {
          price = store.Store.CalculateItemBuyPrice(item, creature.m_idSelf);
        }

        OnStoreRequestSell eventData = new OnStoreRequestSell
        {
          Creature = creature.ToNwObject<NwCreature>(),
          Item = item,
          Store = store,
          Price = price,
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventSell && Hook.CallOriginal(pCreature, oidItemToSell, oidStore).ToBool());
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
    /// <inheritdoc cref="NWN.API.Events.OnStoreRequestSell"/>
    public event Action<OnStoreRequestSell> OnStoreRequestSell
    {
      add => EventService.Subscribe<OnStoreRequestSell, OnStoreRequestSell.Factory>(this, value);
      remove => EventService.Unsubscribe<OnStoreRequestSell, OnStoreRequestSell.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnStoreRequestSell"/>
    public event Action<OnStoreRequestSell> OnStoreRequestSell
    {
      add => EventService.SubscribeAll<OnStoreRequestSell, OnStoreRequestSell.Factory>(value);
      remove => EventService.UnsubscribeAll<OnStoreRequestSell, OnStoreRequestSell.Factory>(value);
    }
  }
}
