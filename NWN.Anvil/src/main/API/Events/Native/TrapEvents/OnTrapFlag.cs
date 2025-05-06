using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a creature attempts to flag a trap.
  /// </summary>
  public sealed class OnTrapFlag : TrapEvent
  {
    public sealed unsafe class Factory : TrapHookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.AIActionFlagTrap> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint> pHook = &OnAIActionFlagTrap;
        Hook = HookService.RequestHook<Functions.CNWSCreature.AIActionFlagTrap>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static uint OnAIActionFlagTrap(void* pCreature, void* pNode)
      {
        OnTrapFlag eventData = HandleExistingTrapEvent<OnTrapFlag>(pCreature, pNode);
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
    /// <inheritdoc cref="Events.OnTrapFlag"/>
    public event Action<OnTrapFlag> OnTrapFlag
    {
      add => EventService.Subscribe<OnTrapFlag, OnTrapFlag.Factory>(this, value);
      remove => EventService.Unsubscribe<OnTrapFlag, OnTrapFlag.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnTrapFlag"/>
    public event Action<OnTrapFlag> OnTrapFlag
    {
      add => EventService.SubscribeAll<OnTrapFlag, OnTrapFlag.Factory>(value);
      remove => EventService.UnsubscribeAll<OnTrapFlag, OnTrapFlag.Factory>(value);
    }
  }
}
