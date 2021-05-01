using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnLevelDown : IEvent
  {
    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context => Creature;

    [NativeFunction(NWNXLib.Functions._ZN17CNWSCreatureStats9LevelDownEP13CNWLevelStats)]
    internal delegate void LevelDownHook(IntPtr pCreatureStats, IntPtr pLevelUpStats);

    internal class Factory : NativeEventFactory<LevelDownHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<LevelDownHook> RequestHook(HookService hookService)
        => hookService.RequestHook<LevelDownHook>(OnLevelDown, HookOrder.Earliest);

      private void OnLevelDown(IntPtr pCreatureStats, IntPtr pLevelUpStats)
      {
        Hook.CallOriginal(pCreatureStats, pLevelUpStats);

        CNWSCreatureStats creatureStats = new CNWSCreatureStats(pCreatureStats, false);

        ProcessEvent(new OnLevelDown
        {
          Creature = creatureStats.m_pBaseCreature.m_idSelf.ToNwObject<NwCreature>(),
        });
      }
    }
  }
}
