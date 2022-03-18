using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using NWN.Native.API;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a creature attempts to unequip an item.
  /// </summary>
  public sealed class OnItemUnequip : IEvent
  {
    /// <summary>
    /// Gets the creature who is uneqipping an item.
    /// </summary>
    public NwCreature Creature { get; private init; }

    /// <summary>
    /// Gets the item being unequipped.
    /// </summary>
    public NwItem Item { get; private init; }

    /// <summary>
    /// Gets or sets whether this item should be prevented from being unequipped.
    /// </summary>
    public bool PreventUnequip { get; set; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<UnequipItemHook> Hook { get; set; }

      private delegate int UnequipItemHook(void* pCreature, uint oidItemToUnequip, uint oidTargetRepository, byte x, byte y, int bMergeIntoRepository, uint oidFeedbackPlayer);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, uint, byte, byte, int, uint, int> pHook = &OnUnequipItem;
        Hook = HookService.RequestHook<UnequipItemHook>(pHook, FunctionsLinux._ZN12CNWSCreature10RunUnequipEjjhhij, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnUnequipItem(void* pCreature, uint oidItemToUnequip, uint oidTargetRepository, byte x, byte y, int bMergeIntoRepository, uint oidFeedbackPlayer)
      {
        OnItemUnequip eventData = ProcessEvent(new OnItemUnequip
        {
          Creature = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>(),
          Item = oidItemToUnequip.ToNwObject<NwItem>(),
        });

        return !eventData.PreventUnequip ? Hook.CallOriginal(pCreature, oidItemToUnequip, oidTargetRepository, x, y, bMergeIntoRepository, oidFeedbackPlayer) : false.ToInt();
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnItemUnequip"/>
    public event Action<OnItemUnequip> OnItemUnequip
    {
      add => EventService.Subscribe<OnItemUnequip, OnItemUnequip.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemUnequip, OnItemUnequip.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnItemUnequip"/>
    public event Action<OnItemUnequip> OnItemUnequip
    {
      add => EventService.SubscribeAll<OnItemUnequip, OnItemUnequip.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemUnequip, OnItemUnequip.Factory>(value);
    }
  }
}
