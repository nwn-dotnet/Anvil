using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnLevelUp : IEvent
  {
    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.LevelUpHook>
    {
      internal delegate void LevelUpHook(void* pCreatureStats, void* pLevelUpStats, byte nDomain1, byte nDomain2, byte nSchool, int bAddStatsToList);

      protected override FunctionHook<LevelUpHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, byte, byte, byte, int, void> pHook = &OnLevelUp;
        return HookService.RequestHook<LevelUpHook>(NWNXLib.Functions._ZN17CNWSCreatureStats7LevelUpEP13CNWLevelStatshhhi, pHook, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnLevelUp(void* pCreatureStats, void* pLevelUpStats, byte nDomain1, byte nDomain2, byte nSchool, int bAddStatsToList)
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
