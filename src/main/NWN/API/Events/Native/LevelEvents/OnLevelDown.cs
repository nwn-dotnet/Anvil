using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnLevelDown : IEvent
  {
    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.LevelDownHook>
    {
      internal delegate void LevelDownHook(void* pCreatureStats, void* pLevelUpStats);

      protected override FunctionHook<LevelDownHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void> pHook = &OnLevelDown;
        return HookService.RequestHook<LevelDownHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats9LevelDownEP13CNWLevelStats, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnLevelDown(void* pCreatureStats, void* pLevelUpStats)
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
