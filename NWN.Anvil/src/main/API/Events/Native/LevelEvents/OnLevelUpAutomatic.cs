using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnLevelUpAutomatic : IEvent
  {
    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.LevelUpAutomaticHook>
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

        CNWSCreatureStats creatureStats = CNWSCreatureStats.FromPointer(pCreatureStats);

        ProcessEvent(new OnLevelUpAutomatic
        {
          Creature = creatureStats.m_pBaseCreature.ToNwObject<NwCreature>(),
        });
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnLevelUpAutomatic"/>
    public event Action<OnLevelUpAutomatic> OnLevelUpAutomatic
    {
      add => EventService.Subscribe<OnLevelUpAutomatic, OnLevelUpAutomatic.Factory>(this, value);
      remove => EventService.Unsubscribe<OnLevelUpAutomatic, OnLevelUpAutomatic.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnLevelUpAutomatic"/>
    public event Action<OnLevelUpAutomatic> OnLevelUpAutomatic
    {
      add => EventService.SubscribeAll<OnLevelUpAutomatic, OnLevelUpAutomatic.Factory>(value);
      remove => EventService.UnsubscribeAll<OnLevelUpAutomatic, OnLevelUpAutomatic.Factory>(value);
    }
  }
}
