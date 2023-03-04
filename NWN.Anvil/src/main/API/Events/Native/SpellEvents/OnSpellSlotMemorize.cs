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

    public NwCreature Creature { get; private init; } = null!;

    public NwDomain? Domain { get; private init; }

    public bool FromClient { get; private init; }

    public MetaMagic MetaMagic { get; private init; }
    public bool PreventMemorize { get; set; }

    public int SlotIndex { get; private init; }

    public NwSpell Spell { get; private init; } = null!;

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<SetMemorizedSpellSlotHook> Hook { get; set; } = null!;

      [NativeFunction("_ZN17CNWSCreatureStats21SetMemorizedSpellSlotEhhjhhi", "")]
      private delegate int SetMemorizedSpellSlotHook(void* pCreatureStats, byte nMultiClass, byte nSpellSlot,
        uint nSpellId, byte nDomainLevel, byte nMetaType, int bFromClient);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, byte, byte, uint, byte, byte, int, int> pHook = &OnSetMemorizedSpellSlot;
        Hook = HookService.RequestHook<SetMemorizedSpellSlotHook>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
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
