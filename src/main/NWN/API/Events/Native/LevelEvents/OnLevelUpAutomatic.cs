using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnLevelUpAutomatic : IEvent
  {
    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context => Creature;

    [NativeFunction(NWNXLib.Functions._ZN17CNWSCreatureStats16LevelUpAutomaticEhih)]
    internal delegate void LevelUpAutomaticHook(IntPtr pCreatureStats, byte nClass, int bReadyAllSpells, byte nPackage);

    internal class Factory : NativeEventFactory<LevelUpAutomaticHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<LevelUpAutomaticHook> RequestHook(HookService hookService)
        => hookService.RequestHook<LevelUpAutomaticHook>(OnLevelUpAutomatic, HookOrder.Earliest);

      private void OnLevelUpAutomatic(IntPtr pCreatureStats, byte nClass, int bReadyAllSpells, byte nPackage)
      {
        Hook.CallOriginal(pCreatureStats, nClass, bReadyAllSpells, nPackage);

        CNWSCreatureStats creatureStats = new CNWSCreatureStats(pCreatureStats, false);

        ProcessEvent(new OnLevelUpAutomatic
        {
          Creature = creatureStats.m_pBaseCreature.m_idSelf.ToNwObject<NwCreature>(),
        });
      }
    }
  }
}
