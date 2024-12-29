using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCombatModeToggle : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    public bool ForceNewMode { get; private init; }

    public ForceNewModeOverride ForceNewModeOverride { get; set; }

    public CombatMode NewMode { get; set; }

    public bool PreventToggle { get; set; }

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.SetCombatMode> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, byte, int, void> pHook = &OnSetCombatMode;
        Hook = HookService.RequestHook<Functions.CNWSCreature.SetCombatMode>(pHook, HookOrder.Early);
        return [Hook];
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

        OnCombatModeToggle eventData = ProcessEvent(EventCallbackType.Before, new OnCombatModeToggle
        {
          Creature = creature.ToNwObject<NwCreature>()!,
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

        ProcessEvent(EventCallbackType.After, eventData);
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
