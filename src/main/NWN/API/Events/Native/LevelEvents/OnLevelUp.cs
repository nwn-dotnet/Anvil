using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnLevelUp : IEvent
  {
    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context => Creature;

    [NativeFunction(NWNXLib.Functions._ZN17CNWSCreatureStats7LevelUpEP13CNWLevelStatshhhi)]
    internal delegate void LevelUpHook(IntPtr pCreatureStats, IntPtr pLevelUpStats, byte nDomain1, byte nDomain2, byte nSchool, int bAddStatsToList);

    internal class Factory : NativeEventFactory<LevelUpHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<LevelUpHook> RequestHook(HookService hookService)
        => hookService.RequestHook<LevelUpHook>(OnLevelUp, HookOrder.Earliest);

      private void OnLevelUp(IntPtr pCreatureStats, IntPtr pLevelUpStats, byte nDomain1, byte nDomain2, byte nSchool, int bAddStatsToList)
      {
        Hook.CallOriginal(pCreatureStats, pLevelUpStats, nDomain1, nDomain2, nSchool, bAddStatsToList);

        CNWSCreatureStats creatureStats = new CNWSCreatureStats(pCreatureStats, false);

        ProcessEvent(new OnLevelUp
        {
          Creature = creatureStats.m_pBaseCreature.m_idSelf.ToNwObject<NwCreature>(),
        });
      }
    }
  }
}
