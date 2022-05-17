using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnItemValidateEquip : IEvent
  {
    public NwItem Item { get; private init; } = null!;

    public EquipValidationResult Result { get; set; }

    public InventorySlot Slot { get; private init; }
    public NwCreature UsedBy { get; private init; } = null!;

    NwObject IEvent.Context => UsedBy;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<CanEquipItemHook> Hook { get; set; } = null!;

      private delegate int CanEquipItemHook(void* pCreature, void* pItem, uint* pEquipToSLot, int bEquipping, int bLoading, int bDisplayFeedback, void* pFeedbackPlayer);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint*, int, int, int, void*, int> pHook = &OnCanEquipItem;
        Hook = HookService.RequestHook<CanEquipItemHook>(pHook, FunctionsLinux._ZN12CNWSCreature12CanEquipItemEP8CNWSItemPjiiiP10CNWSPlayer, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnCanEquipItem(void* pCreature, void* pItem, uint* pEquipToSLot, int bEquipping, int bLoading, int bDisplayFeedback, void* pFeedbackPlayer)
      {
        OnItemValidateEquip eventData = ProcessEvent(new OnItemValidateEquip
        {
          UsedBy = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!,
          Item = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>()!,
          Slot = (InventorySlot)Math.Round(Math.Log2(*pEquipToSLot)),
          Result = (EquipValidationResult)Hook.CallOriginal(pCreature, pItem, pEquipToSLot, bEquipping, bLoading, bDisplayFeedback, pFeedbackPlayer),
        });

        return (int)eventData.Result;
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
