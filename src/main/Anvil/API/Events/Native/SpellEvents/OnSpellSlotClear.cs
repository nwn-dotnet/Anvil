using System;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;

namespace NWN.API.Events
{
  public sealed class OnSpellSlotClear : IEvent
  {
    public bool PreventClear { get; set; }

    public NwCreature Creature { get; private init; }

    public int ClassIndex { get; private init; }

    public int SpellLevel { get; private init; }

    public int SlotIndex { get; private init; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.ClearMemorizedSpellSlotHook>
    {
      internal delegate void ClearMemorizedSpellSlotHook(void* pCreatureStats, byte nMultiClass, byte nSpellLevel, byte nSpellSlot);

      protected override FunctionHook<ClearMemorizedSpellSlotHook> RequestHook()
      {
        delegate* unmanaged<void*, byte, byte, byte, void> pHook = &OnClearMemorizedSpellSlot;
        return HookService.RequestHook<ClearMemorizedSpellSlotHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats23ClearMemorizedSpellSlotEhhh, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnClearMemorizedSpellSlot(void* pCreatureStats, byte nMultiClass, byte nSpellLevel, byte nSpellSlot)
      {
        CNWSCreatureStats creatureStats = CNWSCreatureStats.FromPointer(pCreatureStats);

        OnSpellSlotClear eventData = ProcessEvent(new OnSpellSlotClear
        {
          Creature = creatureStats.m_pBaseCreature.ToNwObject<NwCreature>(),
          ClassIndex = nMultiClass,
          SpellLevel = nSpellLevel,
          SlotIndex = nSpellSlot,
        });

        if (!eventData.PreventClear)
        {
          Hook.CallOriginal(pCreatureStats, nMultiClass, nSpellLevel, nSpellSlot);
        }
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnSpellSlotClear"/>
    public event Action<OnSpellSlotClear> OnSpellSlotClear
    {
      add => EventService.Subscribe<OnSpellSlotClear, OnSpellSlotClear.Factory>(this, value);
      remove => EventService.Unsubscribe<OnSpellSlotClear, OnSpellSlotClear.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnSpellSlotClear"/>
    public event Action<OnSpellSlotClear> OnSpellSlotClear
    {
      add => EventService.SubscribeAll<OnSpellSlotClear, OnSpellSlotClear.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellSlotClear, OnSpellSlotClear.Factory>(value);
    }
  }
}
