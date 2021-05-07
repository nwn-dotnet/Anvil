using System.Runtime.InteropServices;
using NWN.API.Constants;
using NWN.Native.API;
using NWN.Services;

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

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.SetMemorizedSpellSlotHook>
    {
      internal delegate int SetMemorizedSpellSlotHook(void* pCreatureStats, byte nMultiClass, byte nSpellSlot,
        uint nSpellId, byte nDomainLevel, byte nMetaType, int bFromClient);

      protected override FunctionHook<SetMemorizedSpellSlotHook> RequestHook()
      {
        delegate* unmanaged<void*, byte, byte, uint, byte, byte, int, int> pHook = &OnSetMemorizedSpellSlot;
        return HookService.RequestHook<SetMemorizedSpellSlotHook>(NWNXLib.Functions._ZN17CNWSCreatureStats21SetMemorizedSpellSlotEhhjhhi, pHook, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnSetMemorizedSpellSlot(void* pCreatureStats, byte nMultiClass, byte nSpellSlot,
        uint nSpellId, byte nDomainLevel, byte nMetaType, int bFromClient)
      {
        CNWSCreatureStats creatureStats = new CNWSCreatureStats(pCreatureStats, false);

        OnSpellSlotMemorize eventData = ProcessEvent(new OnSpellSlotMemorize
        {
          Creature = creatureStats.m_pBaseCreature.m_idSelf.ToNwObject<NwCreature>(),
          ClassIndex = nMultiClass,
          SlotIndex = nSpellSlot,
          Spell = (Spell)nSpellId,
          Domain = (Domain)nDomainLevel,
          MetaMagic = (MetaMagic)nMetaType,
          FromClient = bFromClient.ToBool()
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
