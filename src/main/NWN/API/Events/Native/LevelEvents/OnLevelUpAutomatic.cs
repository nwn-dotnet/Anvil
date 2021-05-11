using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnLevelUpAutomatic : IEvent
  {
    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.LevelUpAutomaticHook>
    {
      internal delegate void LevelUpAutomaticHook(void* pCreatureStats, byte nClass, int bReadyAllSpells, byte nPackage);

      protected override FunctionHook<LevelUpAutomaticHook> RequestHook()
      {
        delegate* unmanaged<void*, byte, int, byte, void> pHook = &OnLevelUpAutomatic;
        return HookService.RequestHook<LevelUpAutomaticHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats16LevelUpAutomaticEhih, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnLevelUpAutomatic(void* pCreatureStats, byte nClass, int bReadyAllSpells, byte nPackage)
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
