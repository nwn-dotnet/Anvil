using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnLevelDown : IEvent
  {
    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.LevelDownHook>
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

        ProcessEvent(new OnLevelDown
        {
          Creature = CNWSCreatureStats.FromPointer(pCreatureStats)?.m_pBaseCreature.ToNwObject<NwCreature>(),
        });
      }
    }
  }
}
