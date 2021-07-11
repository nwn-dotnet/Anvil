using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
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

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnCombatModeToggle"/>
    public event Action<OnCombatModeToggle> OnCombatModeToggle
    {
      add => EventService.Subscribe<OnCombatModeToggle, OnCombatModeToggle.Factory>(this, value);
      remove => EventService.Unsubscribe<OnCombatModeToggle, OnCombatModeToggle.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnCombatModeToggle"/>
    public event Action<OnCombatModeToggle> OnCombatModeToggle
    {
      add => EventService.SubscribeAll<OnCombatModeToggle, OnCombatModeToggle.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCombatModeToggle, OnCombatModeToggle.Factory>(value);
    }
  }
}
