using System;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;

namespace NWN.API.Events
{
  public sealed class OnLevelUpAutomatic : IEvent
  {
    public NwCreature Creature { get; private init; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

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

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnLevelUpAutomatic"/>
    public event Action<OnLevelUpAutomatic> OnLevelUpAutomatic
    {
      add => EventService.Subscribe<OnLevelUpAutomatic, OnLevelUpAutomatic.Factory>(this, value);
      remove => EventService.Unsubscribe<OnLevelUpAutomatic, OnLevelUpAutomatic.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnLevelUpAutomatic"/>
    public event Action<OnLevelUpAutomatic> OnLevelUpAutomatic
    {
      add => EventService.SubscribeAll<OnLevelUpAutomatic, OnLevelUpAutomatic.Factory>(value);
      remove => EventService.UnsubscribeAll<OnLevelUpAutomatic, OnLevelUpAutomatic.Factory>(value);
    }
  }
}
