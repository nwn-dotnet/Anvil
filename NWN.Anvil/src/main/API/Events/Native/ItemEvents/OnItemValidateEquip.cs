using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when validating whether a creature can equip an item.
  /// </summary>
  public sealed class OnItemValidateEquip : IEvent
  {
    /// <summary>
    /// Gets the item being validated for equip.
    /// </summary>
    public NwItem Item { get; private init; } = null!;

    /// <summary>
    /// Gets or sets the equip validation result.
    /// </summary>
    public EquipValidationResult Result { get; set; }

    /// <summary>
    /// Gets the target inventory slot.
    /// </summary>
    public InventorySlot Slot { get; private init; }

    /// <summary>
    /// Gets the creature attempting to equip the item.
    /// </summary>
    public NwCreature UsedBy { get; private init; } = null!;

    NwObject IEvent.Context => UsedBy;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.CanEquipItem> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint*, int, int, int, void*, int> pHook = &OnCanEquipItem;
        Hook = HookService.RequestHook<Functions.CNWSCreature.CanEquipItem>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnCanEquipItem(void* pCreature, void* pItem, uint* pEquipToSLot, int bEquipping, int bLoading, int bDisplayFeedback, void* pFeedbackPlayer)
      {
        OnItemValidateEquip eventData = ProcessEvent(EventCallbackType.Before, new OnItemValidateEquip
        {
          UsedBy = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!,
          Item = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>()!,
          Slot = (InventorySlot)Math.Round(Math.Log2(*pEquipToSLot)),
          Result = (EquipValidationResult)Hook.CallOriginal(pCreature, pItem, pEquipToSLot, bEquipping, bLoading, bDisplayFeedback, pFeedbackPlayer),
        });

        int retVal = (int)eventData.Result;
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
    /// <inheritdoc cref="Events.OnItemValidateEquip"/>
    public event Action<OnItemValidateEquip> OnItemValidateEquip
    {
      add => EventService.Subscribe<OnItemValidateEquip, OnItemValidateEquip.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemValidateEquip, OnItemValidateEquip.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnItemValidateEquip"/>
    public event Action<OnItemValidateEquip> OnItemValidateEquip
    {
      add => EventService.SubscribeAll<OnItemValidateEquip, OnItemValidateEquip.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemValidateEquip, OnItemValidateEquip.Factory>(value);
    }
  }
}
