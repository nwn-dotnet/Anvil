using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnStoreRequestSell : IEvent
  {
    public NwCreature Creature { get; private init; }

    public NwItem Item { get; private init; }
    public bool PreventSell { get; set; }

    public int Price { get; private init; }

    public Lazy<bool> Result { get; private set; }

    public NwStore Store { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      internal delegate int RequestSellHook(void* pCreature, uint oidItemToBuy, uint oidStore);

      private static FunctionHook<RequestSellHook> Hook { get; set; }

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, uint, int> pHook = &OnRequestSell;
        Hook = HookService.RequestHook<RequestSellHook>(pHook, FunctionsLinux._ZN12CNWSCreature11RequestSellEjj, HookOrder.Early);
        return new IDisposable[] { Hook };
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

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnStoreRequestSell"/>
    public event Action<OnStoreRequestSell> OnStoreRequestSell
    {
      add => EventService.Subscribe<OnStoreRequestSell, OnStoreRequestSell.Factory>(this, value);
      remove => EventService.Unsubscribe<OnStoreRequestSell, OnStoreRequestSell.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnStoreRequestSell"/>
    public event Action<OnStoreRequestSell> OnStoreRequestSell
    {
      add => EventService.SubscribeAll<OnStoreRequestSell, OnStoreRequestSell.Factory>(value);
      remove => EventService.UnsubscribeAll<OnStoreRequestSell, OnStoreRequestSell.Factory>(value);
    }
  }
}
