using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when an effect is checking for creature immunities.<br/>
  /// Use this event to force an effect to bypass creature immunities.
  /// </summary>
  public sealed class OnCheckEffectImmunity : IEvent
  {
    /// <summary>
    /// The creature that the effect is being applied to.
    /// </summary>
    public NwCreature Creature { get; private init; }

    /// <summary>
    /// The type of immunity being checked.
    /// </summary>
    public ImmunityType ImmunityType { get; private init; }

    /// <summary>
    /// Gets or sets whether the specified immunity should be ignored, and the effect applied regardless.
    /// </summary>
    public bool Bypass { get; set; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.GetEffectImmunityHook>
    {
      internal delegate int GetEffectImmunityHook(void* pStats, byte nType, void* pVerses, int bConsiderFeats);

      protected override FunctionHook<GetEffectImmunityHook> RequestHook()
      {
        delegate* unmanaged<void*, byte, void*, int, int> pHook = &OnGetEffectImmunity;
        return HookService.RequestHook<GetEffectImmunityHook>(pHook, FunctionsLinux._ZN17CNWSCreatureStats17GetEffectImmunityEhP12CNWSCreaturei, HookOrder.Late);
      }

      [UnmanagedCallersOnly]
      private static int OnGetEffectImmunity(void* pStats, byte nType, void* pVerses, int bConsiderFeats)
      {
        CNWSCreatureStats creatureStats = CNWSCreatureStats.FromPointer(pStats);
        OnCheckEffectImmunity eventData = ProcessEvent(new OnCheckEffectImmunity
        {
          Creature = creatureStats.m_pBaseCreature.ToNwObject<NwCreature>(),
          ImmunityType = (ImmunityType)nType,
        });

        return eventData.Bypass ? false.ToInt() : Hook.CallOriginal(pStats, nType, pVerses, bConsiderFeats);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnCheckEffectImmunity"/>
    public event Action<OnCheckEffectImmunity> OnCheckEffectImmunity
    {
      add => EventService.Subscribe<OnCheckEffectImmunity, OnCheckEffectImmunity.Factory>(this, value);
      remove => EventService.Unsubscribe<OnCheckEffectImmunity, OnCheckEffectImmunity.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnCheckEffectImmunity"/>
    public event Action<OnCheckEffectImmunity> OnCheckEffectImmunity
    {
      add => EventService.SubscribeAll<OnCheckEffectImmunity, OnCheckEffectImmunity.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCheckEffectImmunity, OnCheckEffectImmunity.Factory>(value);
    }
  }
}


