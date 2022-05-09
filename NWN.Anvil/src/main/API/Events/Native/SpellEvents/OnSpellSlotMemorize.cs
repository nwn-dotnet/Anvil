using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnSpellSlotMemorize : IEvent
  {
    public int ClassIndex { get; private init; }

    public NwCreature Creature { get; private init; }

    public Domain Domain { get; private init; }

    public bool FromClient { get; private init; }

    public MetaMagic MetaMagic { get; private init; }
    public bool PreventMemorize { get; set; }

    public int SlotIndex { get; private init; }

    public NwSpell Spell { get; private init; }

    NwObject? IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<SetMemorizedSpellSlotHook> Hook { get; set; }

      private delegate int SetMemorizedSpellSlotHook(void* pCreatureStats, byte nMultiClass, byte nSpellSlot,
        uint nSpellId, byte nDomainLevel, byte nMetaType, int bFromClient);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, byte, byte, uint, byte, byte, int, int> pHook = &OnSetMemorizedSpellSlot;
        Hook = HookService.RequestHook<SetMemorizedSpellSlotHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats21SetMemorizedSpellSlotEhhjhhi, HookOrder.Early);
        return new IDisposable[] { Hook };
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
          Spell = NwSpell.FromSpellId((int)nSpellId),
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

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnSpellSlotMemorize"/>
    public event Action<OnSpellSlotMemorize> OnSpellSlotMemorize
    {
      add => EventService.Subscribe<OnSpellSlotMemorize, OnSpellSlotMemorize.Factory>(this, value);
      remove => EventService.Unsubscribe<OnSpellSlotMemorize, OnSpellSlotMemorize.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnSpellSlotMemorize"/>
    public event Action<OnSpellSlotMemorize> OnSpellSlotMemorize
    {
      add => EventService.SubscribeAll<OnSpellSlotMemorize, OnSpellSlotMemorize.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellSlotMemorize, OnSpellSlotMemorize.Factory>(value);
    }
  }
}
