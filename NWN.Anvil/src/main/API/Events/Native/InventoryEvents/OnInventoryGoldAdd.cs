using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnInventoryGoldAdd : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    public int Gold { get; private init; }

    public bool PreventGoldAdd { get; set; }

    NwObject? IEvent.Context => null;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.AddGold> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, int, void> pHook = &OnAddGold;
        Hook = HookService.RequestHook<Functions.CNWSCreature.AddGold>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static void OnAddGold(void* pCreature, int nGold, int bDisplayFeedback)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnInventoryGoldAdd eventData = ProcessEvent(EventCallbackType.Before, new OnInventoryGoldAdd
        {
          Creature = creature.ToNwObject<NwCreature>()!,
          Gold = nGold,
        });

        if (!eventData.PreventGoldAdd)
        {
          Hook.CallOriginal(pCreature, nGold, bDisplayFeedback);
        }

        ProcessEvent(EventCallbackType.After, eventData);
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
