using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnInventoryGoldAdd : IEvent
  {
    public NwCreature Creature { get; private init; }

    public int Gold { get; private init; }

    public bool PreventGoldAdd { get; set; }

    NwObject? IEvent.Context => null;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<AddGoldHook> Hook { get; set; }

      private delegate void AddGoldHook(void* pCreature, int nGold, int bDisplayFeedback);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, int, void> pHook = &OnAddGold;
        Hook = HookService.RequestHook<AddGoldHook>(pHook, FunctionsLinux._ZN12CNWSCreature7AddGoldEii, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnAddGold(void* pCreature, int nGold, int bDisplayFeedback)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnInventoryGoldAdd eventData = ProcessEvent(new OnInventoryGoldAdd
        {
          Creature = creature.ToNwObject<NwCreature>(),
          Gold = nGold,
        });

        if (!eventData.PreventGoldAdd)
        {
          Hook.CallOriginal(pCreature, nGold, bDisplayFeedback);
        }
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnInventoryGoldAdd"/>
    public event Action<OnInventoryGoldAdd> OnInventoryGoldAdd
    {
      add => EventService.Subscribe<OnInventoryGoldAdd, OnInventoryGoldAdd.Factory>(this, value);
      remove => EventService.Unsubscribe<OnInventoryGoldAdd, OnInventoryGoldAdd.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnInventoryGoldAdd"/>
    public event Action<OnInventoryGoldAdd> OnInventoryGoldAdd
    {
      add => EventService.SubscribeAll<OnInventoryGoldAdd, OnInventoryGoldAdd.Factory>(value);
      remove => EventService.UnsubscribeAll<OnInventoryGoldAdd, OnInventoryGoldAdd.Factory>(value);
    }
  }
}
