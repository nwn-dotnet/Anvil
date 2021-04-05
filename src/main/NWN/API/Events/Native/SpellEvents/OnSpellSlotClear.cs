using System;
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

    [NativeFunction(NWNXLib.Functions._ZN17CNWSCreatureStats23ClearMemorizedSpellSlotEhhh)]
    internal delegate void ClearMemorizedSpellSlotHook(IntPtr pCreatureStats, byte nMultiClass, byte nSpellLevel, byte nSpellSlot);

    internal class Factory : NativeEventFactory<ClearMemorizedSpellSlotHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<ClearMemorizedSpellSlotHook> RequestHook(HookService hookService)
        => hookService.RequestHook<ClearMemorizedSpellSlotHook>(OnClearMemorizedSpellSlot, HookOrder.Early);

      private void OnClearMemorizedSpellSlot(IntPtr pCreatureStats, byte nMultiClass, byte nSpellLevel, byte nSpellSlot)
      {
        CNWSCreatureStats creatureStats = new CNWSCreatureStats(pCreatureStats, false);

        OnSpellSlotClear eventData = ProcessEvent(new OnSpellSlotClear
        {
          Creature = creatureStats.m_pBaseCreature.m_idSelf.ToNwObject<NwCreature>(),
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
