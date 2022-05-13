using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnLevelUp : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<LevelUpHook> Hook { get; set; } = null!;

      private delegate void LevelUpHook(void* pCreatureStats, void* pLevelUpStats, byte nDomain1, byte nDomain2, byte nSchool, int bAddStatsToList);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, byte, byte, byte, int, void> pHook = &OnLevelUp;
        Hook = HookService.RequestHook<LevelUpHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats7LevelUpEP13CNWLevelStatshhhi, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnLevelUp(void* pCreatureStats, void* pLevelUpStats, byte nDomain1, byte nDomain2, byte nSchool, int bAddStatsToList)
      {
        Hook.CallOriginal(pCreatureStats, pLevelUpStats, nDomain1, nDomain2, nSchool, bAddStatsToList);

        CNWSCreatureStats creatureStats = CNWSCreatureStats.FromPointer(pCreatureStats);

        ProcessEvent(new OnLevelUp
        {
          Creature = creatureStats.m_pBaseCreature.ToNwObject<NwCreature>()!,
        });
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnLevelUp"/>
    public event Action<OnLevelUp> OnLevelUp
    {
      add => EventService.Subscribe<OnLevelUp, OnLevelUp.Factory>(this, value);
      remove => EventService.Unsubscribe<OnLevelUp, OnLevelUp.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnLevelUp"/>
    public event Action<OnLevelUp> OnLevelUp
    {
      add => EventService.SubscribeAll<OnLevelUp, OnLevelUp.Factory>(value);
      remove => EventService.UnsubscribeAll<OnLevelUp, OnLevelUp.Factory>(value);
    }
  }
}
