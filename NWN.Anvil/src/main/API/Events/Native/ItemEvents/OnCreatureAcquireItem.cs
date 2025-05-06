using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a creature acquires an item. This event is skippable.
  /// </summary>
  public sealed class OnCreatureAcquireItem : IEvent
  {
    /// <summary>
    /// Gets the item that will be acquired.
    /// </summary>
    public NwItem Item { get; private init; } = null!;

    /// <summary>
    /// Gets the creature that will acquire the item.
    /// </summary>
    public NwCreature AcquiredBy { get; private init; } = null!;

    /// <summary>
    /// Gets the last possessor of the item.
    /// </summary>
    public NwGameObject? AcquiredFrom { get; private init; }

    /// <summary>
    /// Gets if the creature was able to successfully acquire the item.
    /// </summary>
    public Lazy<bool> Result { get; private set; } = null!;

    /// <summary>
    /// Gets or sets if the event should be skipped, and the creature prevented from acquiring the item.
    /// </summary>
    public bool Skip { get; set; }

    NwObject IEvent.Context => AcquiredBy;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.AcquireItem> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void**, uint, uint, byte, byte, int, int, int> pHook = &OnAcquireItem;
        Hook = HookService.RequestHook<Functions.CNWSCreature.AcquireItem>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnAcquireItem(void* pCreature, void** pItem, uint oidPossessor, uint oidTargetRepository, byte x, byte y, int bOriginatingFromScript, int bDisplayFeedback)
      {
        if (pItem == null)
        {
          return Hook.CallOriginal(pCreature, pItem, oidPossessor, oidTargetRepository, x, y, bOriginatingFromScript, bDisplayFeedback);
        }

        CNWSItem cItem = CNWSItem.FromPointer(*pItem);
        if (cItem == null)
        {
          return Hook.CallOriginal(pCreature, pItem, oidPossessor, oidTargetRepository, x, y, bOriginatingFromScript, bDisplayFeedback);
        }

        NwItem item = cItem.ToNwObject<NwItem>()!;
        OnCreatureAcquireItem eventData = new OnCreatureAcquireItem
        {
          AcquiredBy = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!,
          Item = item,
          AcquiredFrom = oidPossessor.ToNwObject<NwGameObject>(),
        };

        eventData.Result = new Lazy<bool>(() => !eventData.Skip && Hook.CallOriginal(pCreature, pItem, oidPossessor, oidTargetRepository, x, y, bOriginatingFromScript, bDisplayFeedback).ToBool());
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
    /// <inheritdoc cref="OnCreatureAcquireItem"/>
    public event Action<OnCreatureAcquireItem> OnCreatureAcquireItem
    {
      add => EventService.Subscribe<OnCreatureAcquireItem, OnCreatureAcquireItem.Factory>(this, value);
      remove => EventService.Unsubscribe<OnCreatureAcquireItem, OnCreatureAcquireItem.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="OnCreatureAcquireItem"/>
    public event Action<OnCreatureAcquireItem> OnCreatureAcquireItem
    {
      add => EventService.SubscribeAll<OnCreatureAcquireItem, OnCreatureAcquireItem.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCreatureAcquireItem, OnCreatureAcquireItem.Factory>(value);
    }
  }
}
