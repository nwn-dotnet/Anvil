using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnStoreRequestBuy : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    public NwItem? Item { get; private init; }
    public bool PreventBuy { get; set; }

    public int Price { get; private init; }

    public Lazy<bool> Result { get; private set; } = null!;

    public NwStore? Store { get; private init; }

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.RequestBuy> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, uint, uint, int> pHook = &OnRequestBuy;
        Hook = HookService.RequestHook<Functions.CNWSCreature.RequestBuy>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnRequestBuy(void* pCreature, uint oidItemToBuy, uint oidStore, uint oidDesiredRepository)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
        NwItem? item = oidItemToBuy.ToNwObject<NwItem>();
        NwStore? store = oidStore.ToNwObject<NwStore>();

        int price = 0;

        if (store != null && item != null)
        {
          price = store.Store.CalculateItemSellPrice(item, creature.m_idSelf);
        }

        OnStoreRequestBuy eventData = new OnStoreRequestBuy
        {
          Creature = creature.ToNwObject<NwCreature>()!,
          Item = item,
          Store = store,
          Price = price,
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventBuy && Hook.CallOriginal(pCreature, oidItemToBuy, oidStore, oidDesiredRepository).ToBool());
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
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnStoreRequestBuy"/>
    public event Action<OnStoreRequestBuy> OnStoreRequestBuy
    {
      add => EventService.Subscribe<OnStoreRequestBuy, OnStoreRequestBuy.Factory>(this, value);
      remove => EventService.Unsubscribe<OnStoreRequestBuy, OnStoreRequestBuy.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnStoreRequestBuy"/>
    public event Action<OnStoreRequestBuy> OnStoreRequestBuy
    {
      add => EventService.SubscribeAll<OnStoreRequestBuy, OnStoreRequestBuy.Factory>(value);
      remove => EventService.UnsubscribeAll<OnStoreRequestBuy, OnStoreRequestBuy.Factory>(value);
    }
  }
}
