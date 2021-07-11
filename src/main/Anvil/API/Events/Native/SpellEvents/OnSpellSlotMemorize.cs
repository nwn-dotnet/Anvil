using System;
using System.Runtime.InteropServices;
using Anvil.Services;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Native.API;

namespace NWN.API.Events
{
  public sealed class OnSpellSlotMemorize : IEvent
  {
    public bool PreventMemorize { get; set; }

    public NwCreature Creature { get; private init; }

    public int ClassIndex { get; private init; }

    public int SlotIndex { get; private init; }

    public Spell Spell { get; private init; }

    public Domain Domain { get; private init; }

    public MetaMagic MetaMagic { get; private init; }

    public bool FromClient { get; private init; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.SetMemorizedSpellSlotHook>
    {
      internal delegate int SetMemorizedSpellSlotHook(void* pCreatureStats, byte nMultiClass, byte nSpellSlot,
        uint nSpellId, byte nDomainLevel, byte nMetaType, int bFromClient);

      protected override FunctionHook<SetMemorizedSpellSlotHook> RequestHook()
      {
        delegate* unmanaged<void*, byte, byte, uint, byte, byte, int, int> pHook = &OnSetMemorizedSpellSlot;
        return HookService.RequestHook<SetMemorizedSpellSlotHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats21SetMemorizedSpellSlotEhhjhhi, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnSetMemorizedSpellSlot(void* pCreatureStats, byte nMultiClass, byte nSpellSlot,
        uint nSpellId, byte nDomainLevel, byte nMetaType, int bFromClient)
      {
        CNWSCreatureStats creatureStats = CNWSCreatureStats.FromPointer(pCreatureStats);

        OnSpellSlotMemorize eventData = ProcessEvent(new OnSpellSlotMemorize
        {
          Creature = creatureStats.m_pBaseCreature.ToNwObject<NwCreature>(),
          ClassIndex = nMultiClass,
          SlotIndex = nSpellSlot,
          Spell = (Spell)nSpellId,
          Domain = (Domain)nDomainLevel,
          MetaMagic = (MetaMagic)nMetaType,
          FromClient = bFromClient.ToBool(),
        });

        if (!eventData.PreventMemorize)
        {
          return Hook.CallOriginal(pCreatureStats, nMultiClass, nSpellSlot, nSpellId, nDomainLevel, nMetaType, bFromClient);
        }

        return false.ToInt();
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnSpellSlotMemorize"/>
    public event Action<OnSpellSlotMemorize> OnSpellSlotMemorize
    {
      add => EventService.Subscribe<OnSpellSlotMemorize, OnSpellSlotMemorize.Factory>(this, value);
      remove => EventService.Unsubscribe<OnSpellSlotMemorize, OnSpellSlotMemorize.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnSpellSlotMemorize"/>
    public event Action<OnSpellSlotMemorize> OnSpellSlotMemorize
    {
      add => EventService.SubscribeAll<OnSpellSlotMemorize, OnSpellSlotMemorize.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellSlotMemorize, OnSpellSlotMemorize.Factory>(value);
    }
  }
}
