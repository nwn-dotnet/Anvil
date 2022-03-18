using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnInventoryGoldRemove : IEvent
  {
    public NwCreature Creature { get; private init; }

    public int Gold { get; private init; }

    public bool PreventGoldRemove { get; set; }

    NwObject IEvent.Context => null;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private delegate void RemoveGoldHook(void* pCreature, int nGold, int bDisplayFeedback);

      private static FunctionHook<RemoveGoldHook> Hook { get; set; }

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, int, void> pHook = &OnRemoveGold;
        Hook = HookService.RequestHook<RemoveGoldHook>(pHook, FunctionsLinux._ZN12CNWSCreature10RemoveGoldEii, HookOrder.Early);
        return new IDisposable[] { Hook };
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

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnInventoryGoldRemove"/>
    public event Action<OnInventoryGoldRemove> OnInventoryGoldRemove
    {
      add => EventService.Subscribe<OnInventoryGoldRemove, OnInventoryGoldRemove.Factory>(this, value);
      remove => EventService.Unsubscribe<OnInventoryGoldRemove, OnInventoryGoldRemove.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnInventoryGoldRemove"/>
    public event Action<OnInventoryGoldRemove> OnInventoryGoldRemove
    {
      add => EventService.SubscribeAll<OnInventoryGoldRemove, OnInventoryGoldRemove.Factory>(value);
      remove => EventService.UnsubscribeAll<OnInventoryGoldRemove, OnInventoryGoldRemove.Factory>(value);
    }
  }
}
