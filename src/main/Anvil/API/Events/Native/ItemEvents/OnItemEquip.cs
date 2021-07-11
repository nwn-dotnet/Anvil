using System;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;
using InventorySlot = Anvil.API.InventorySlot;

namespace NWN.API.Events
{
  public sealed class OnItemEquip : IEvent
  {
    public NwCreature EquippedBy { get; private init; }

    public NwItem Item { get; private init; }

    public InventorySlot Slot { get; private init; }

    public bool PreventEquip { get; set; }

    public Lazy<bool> Result { get; private set; }

    NwObject IEvent.Context
    {
      get => EquippedBy;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.RunEquipHook>
    {
      internal delegate int RunEquipHook(void* pCreature, uint oidItemToEquip, uint nInventorySlot, uint oidFeedbackPlayer);

      protected override FunctionHook<RunEquipHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, uint, uint, int> pHook = &OnRunEquip;
        return HookService.RequestHook<RunEquipHook>(pHook, FunctionsLinux._ZN12CNWSCreature8RunEquipEjjj, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnRunEquip(void* pCreature, uint oidItemToEquip, uint nInventorySlot, uint oidFeedbackPlayer)
      {
        uint slot = nInventorySlot;
        byte slotId = 0;

        while ((slot >>= 1) != 0)
        {
          slotId++;
        }

        OnItemEquip eventData = new OnItemEquip
        {
          EquippedBy = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>(),
          Item = oidItemToEquip.ToNwObject<NwItem>(),
          Slot = (InventorySlot)slotId,
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventEquip && Hook.CallOriginal(pCreature, oidItemToEquip, nInventorySlot, oidFeedbackPlayer).ToBool());
        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnItemEquip"/>
    public event Action<OnItemEquip> OnItemEquip
    {
      add => EventService.Subscribe<OnItemEquip, OnItemEquip.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemEquip, OnItemEquip.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnItemEquip"/>
    public event Action<OnItemEquip> OnItemEquip
    {
      add => EventService.SubscribeAll<OnItemEquip, OnItemEquip.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemEquip, OnItemEquip.Factory>(value);
    }
  }
}
