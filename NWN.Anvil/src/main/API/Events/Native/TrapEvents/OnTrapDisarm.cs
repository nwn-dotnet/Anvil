using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a creature attempts to disarm a trap.
  /// </summary>
  public sealed class OnTrapDisarm : TrapEvent
  {
    public sealed unsafe class Factory : TrapHookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.AIActionDisarmTrap> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint> pHook = &OnAIActionDisarmTrap;
        Hook = HookService.RequestHook<Functions.CNWSCreature.AIActionDisarmTrap>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static uint OnAIActionDisarmTrap(void* pCreature, void* pNode)
      {
        OnTrapDisarm eventData = HandleExistingTrapEvent<OnTrapDisarm>(pCreature, pNode);
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
    /// <inheritdoc cref="Events.OnTrapDisarm"/>
    public event Action<OnTrapDisarm> OnTrapDisarm
    {
      add => EventService.Subscribe<OnTrapDisarm, OnTrapDisarm.Factory>(this, value);
      remove => EventService.Unsubscribe<OnTrapDisarm, OnTrapDisarm.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnTrapDisarm"/>
    public event Action<OnTrapDisarm> OnTrapDisarm
    {
      add => EventService.SubscribeAll<OnTrapDisarm, OnTrapDisarm.Factory>(value);
      remove => EventService.UnsubscribeAll<OnTrapDisarm, OnTrapDisarm.Factory>(value);
    }
  }
}
