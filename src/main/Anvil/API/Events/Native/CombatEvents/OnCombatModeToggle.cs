using System;
using System.Runtime.InteropServices;
using NWN.API.Events;
using NWN.Native.API;
using NWN.Services;
using CombatMode = NWN.API.Constants.CombatMode;

namespace NWN.API.Events
{
  public sealed class OnCombatModeToggle : IEvent
  {
    public NwCreature Creature { get; private init; }

    public CombatMode NewMode { get; set; }

    public bool ForceNewMode { get; private init; }

    public ForceNewModeOverride ForceNewModeOverride { get; set; }

    public bool PreventToggle { get; set; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.SetCombatModeHook>
    {
      internal delegate void SetCombatModeHook(void* pCreature, byte nNewMode, int bForceNewMode);

      protected override FunctionHook<SetCombatModeHook> RequestHook()
      {
        delegate* unmanaged<void*, byte, int, void> pHook = &OnSetCombatMode;
        return HookService.RequestHook<SetCombatModeHook>(pHook, FunctionsLinux._ZN12CNWSCreature13SetCombatModeEhi, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static void OnSetCombatMode(void* pCreature, byte nNewMode, int bForceNewMode)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
        if (creature == null)
        {
          Hook.CallOriginal(pCreature, nNewMode, bForceNewMode);
          return;
        }

        OnCombatModeToggle eventData = ProcessEvent(new OnCombatModeToggle
        {
          Creature = creature.ToNwObject<NwCreature>(),
          NewMode = (CombatMode)nNewMode,
          ForceNewMode = bForceNewMode.ToBool(),
        });

        bForceNewMode = eventData.ForceNewModeOverride switch
        {
          ForceNewModeOverride.Force => true.ToInt(),
          ForceNewModeOverride.DontForce => false.ToInt(),
          _ => bForceNewMode,
        };

        if (!eventData.PreventToggle)
        {
          Hook.CallOriginal(pCreature, (byte)eventData.NewMode, bForceNewMode);
        }
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnCombatModeToggle"/>
    public event Action<OnCombatModeToggle> OnCombatModeToggle
    {
      add => EventService.Subscribe<OnCombatModeToggle, OnCombatModeToggle.Factory>(this, value);
      remove => EventService.Unsubscribe<OnCombatModeToggle, OnCombatModeToggle.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnCombatModeToggle"/>
    public event Action<OnCombatModeToggle> OnCombatModeToggle
    {
      add => EventService.SubscribeAll<OnCombatModeToggle, OnCombatModeToggle.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCombatModeToggle, OnCombatModeToggle.Factory>(value);
    }
  }
}
