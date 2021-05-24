using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnSpellSlotClear : IEvent
  {
    public bool PreventClear { get; set; }

    public NwCreature Creature { get; private init; }

    public int ClassIndex { get; private init; }

    public int SpellLevel { get; private init; }

    public int SlotIndex { get; private init; }

    NwObject IEvent.Context => Creature;

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
          SlotIndex = nSpellSlot
        });

        if (!eventData.PreventClear)
        {
          Hook.CallOriginal(pCreatureStats, nMultiClass, nSpellLevel, nSpellSlot);
        }
      }
    }
  }
}
