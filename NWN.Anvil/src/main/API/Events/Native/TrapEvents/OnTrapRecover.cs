using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a creature attempts to recover a trap.
  /// </summary>
  public sealed class OnTrapRecover : TrapEvent
  {
    public sealed unsafe class Factory : TrapHookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.AIActionRecoverTrap> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint> pHook = &OnAIActionRecoverTrap;
        Hook = HookService.RequestHook<Functions.CNWSCreature.AIActionRecoverTrap>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static uint OnAIActionRecoverTrap(void* pCreature, void* pNode)
      {
        OnTrapRecover eventData = HandleExistingTrapEvent<OnTrapRecover>(pCreature, pNode);
        if (eventData.ResultOverride is not (ActionState.Complete or ActionState.Failed))
        {
          return Hook.CallOriginal(pCreature, pNode);
        }

        eventData.Result = eventData.ResultOverride.Value;
        ProcessEvent(EventCallbackType.After, eventData);

        return (uint)eventData.Result;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnTrapRecover"/>
    public event Action<OnTrapRecover> OnTrapRecover
    {
      add => EventService.Subscribe<OnTrapRecover, OnTrapRecover.Factory>(this, value);
      remove => EventService.Unsubscribe<OnTrapRecover, OnTrapRecover.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnTrapRecover"/>
    public event Action<OnTrapRecover> OnTrapRecover
    {
      add => EventService.SubscribeAll<OnTrapRecover, OnTrapRecover.Factory>(value);
      remove => EventService.UnsubscribeAll<OnTrapRecover, OnTrapRecover.Factory>(value);
    }
  }
}
