using System;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;

namespace NWN.API.Events
{
  public sealed class OnInventoryGoldRemove : IEvent
  {
    public NwCreature Creature { get; private init; }

    public int Gold { get; private init; }

    public bool PreventGoldRemove { get; set; }

    NwObject IEvent.Context
    {
      get => null;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.RemoveGoldHook>
    {
      internal delegate void RemoveGoldHook(void* pCreature, int nGold, int bDisplayFeedback);

      protected override FunctionHook<RemoveGoldHook> RequestHook()
      {
        delegate* unmanaged<void*, int, int, void> pHook = &OnRemoveGold;
        return HookService.RequestHook<RemoveGoldHook>(pHook, FunctionsLinux._ZN12CNWSCreature10RemoveGoldEii, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnRemoveGold(void* pCreature, int nGold, int bDisplayFeedback)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnInventoryGoldRemove eventData = ProcessEvent(new OnInventoryGoldRemove
        {
          Creature = creature.ToNwObject<NwCreature>(),
          Gold = nGold,
        });

        if (!eventData.PreventGoldRemove)
        {
          Hook.CallOriginal(pCreature, nGold, bDisplayFeedback);
        }
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnInventoryGoldRemove"/>
    public event Action<OnInventoryGoldRemove> OnInventoryGoldRemove
    {
      add => EventService.Subscribe<OnInventoryGoldRemove, OnInventoryGoldRemove.Factory>(this, value);
      remove => EventService.Unsubscribe<OnInventoryGoldRemove, OnInventoryGoldRemove.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnInventoryGoldRemove"/>
    public event Action<OnInventoryGoldRemove> OnInventoryGoldRemove
    {
      add => EventService.SubscribeAll<OnInventoryGoldRemove, OnInventoryGoldRemove.Factory>(value);
      remove => EventService.UnsubscribeAll<OnInventoryGoldRemove, OnInventoryGoldRemove.Factory>(value);
    }
  }
}
