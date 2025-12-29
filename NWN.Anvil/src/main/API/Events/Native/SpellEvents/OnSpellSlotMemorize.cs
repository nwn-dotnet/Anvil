using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a creature memorizes a spell into a slot.
  /// </summary>
  public sealed class OnSpellSlotMemorize : IEvent
  {
    /// <summary>
    /// Gets the caster's class index used for the spell.
    /// </summary>
    public int ClassIndex { get; private init; }

    /// <summary>
    /// Gets the creature memorizing the spell.
    /// </summary>
    public NwCreature Creature { get; private init; } = null!;

    /// <summary>
    /// Gets the domain level used, if applicable.
    /// </summary>
    public NwDomain? Domain { get; private init; }

    /// <summary>
    /// Gets whether the request originated from the client.
    /// </summary>
    public bool FromClient { get; private init; }

    /// <summary>
    /// Gets the metamagic applied to the memorized spell.
    /// </summary>
    public MetaMagic MetaMagic { get; private init; }

    /// <summary>
    /// Set to true to prevent memorizing the spell slot.
    /// </summary>
    public bool PreventMemorize { get; set; }

    /// <summary>
    /// Gets the target spell slot index.
    /// </summary>
    public int SlotIndex { get; private init; }

    /// <summary>
    /// Gets the spell being memorized.
    /// </summary>
    public NwSpell Spell { get; private init; } = null!;

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreatureStats.SetMemorizedSpellSlot> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, byte, byte, uint, byte, byte, int, int> pHook = &OnSetMemorizedSpellSlot;
        Hook = HookService.RequestHook<Functions.CNWSCreatureStats.SetMemorizedSpellSlot>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnSetMemorizedSpellSlot(void* pCreatureStats, byte nMultiClass, byte nSpellSlot,
        uint nSpellId, byte nDomainLevel, byte nMetaType, int bFromClient)
      {
        CNWSCreatureStats creatureStats = CNWSCreatureStats.FromPointer(pCreatureStats);

        OnSpellSlotMemorize eventData = ProcessEvent(EventCallbackType.Before, new OnSpellSlotMemorize
        {
          Creature = creatureStats.m_pBaseCreature.ToNwObject<NwCreature>()!,
          ClassIndex = nMultiClass,
          SlotIndex = nSpellSlot,
          Spell = NwSpell.FromSpellId((int)nSpellId)!,
          Domain = NwDomain.FromDomainId(nDomainLevel),
          MetaMagic = (MetaMagic)nMetaType,
          FromClient = bFromClient.ToBool(),
        });

        int retVal = false.ToInt();
        if (!eventData.PreventMemorize)
        {
          retVal = Hook.CallOriginal(pCreatureStats, nMultiClass, nSpellSlot, nSpellId, nDomainLevel, nMetaType, bFromClient);
        }

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
