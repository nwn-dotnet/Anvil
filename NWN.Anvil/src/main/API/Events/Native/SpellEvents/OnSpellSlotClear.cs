using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnSpellSlotClear : IEvent
  {
    public int ClassIndex { get; private init; }

    public NwCreature Creature { get; private init; } = null!;
    public bool PreventClear { get; set; }

    public int SlotIndex { get; private init; }

    public int SpellLevel { get; private init; }

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreatureStats.ClearMemorizedSpellSlot> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, byte, byte, byte, void> pHook = &OnClearMemorizedSpellSlot;
        Hook = HookService.RequestHook<Functions.CNWSCreatureStats.ClearMemorizedSpellSlot>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static void OnClearMemorizedSpellSlot(void* pCreatureStats, byte nMultiClass, byte nSpellLevel, byte nSpellSlot)
      {
        CNWSCreatureStats creatureStats = CNWSCreatureStats.FromPointer(pCreatureStats);

        OnSpellSlotClear eventData = ProcessEvent(EventCallbackType.Before, new OnSpellSlotClear
        {
          Creature = creatureStats.m_pBaseCreature.ToNwObject<NwCreature>()!,
          ClassIndex = nMultiClass,
          SpellLevel = nSpellLevel,
          SlotIndex = nSpellSlot,
        });

        if (!eventData.PreventClear)
        {
          Hook.CallOriginal(pCreatureStats, nMultiClass, nSpellLevel, nSpellSlot);
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
    /// <inheritdoc cref="Events.OnSpellSlotClear"/>
    public event Action<OnSpellSlotClear> OnSpellSlotClear
    {
      add => EventService.Subscribe<OnSpellSlotClear, OnSpellSlotClear.Factory>(this, value);
      remove => EventService.Unsubscribe<OnSpellSlotClear, OnSpellSlotClear.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnSpellSlotClear"/>
    public event Action<OnSpellSlotClear> OnSpellSlotClear
    {
      add => EventService.SubscribeAll<OnSpellSlotClear, OnSpellSlotClear.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellSlotClear, OnSpellSlotClear.Factory>(value);
    }
  }
}
