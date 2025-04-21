using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a creature attempts to examine a trap.
  /// </summary>
  public sealed class OnTrapExamine : TrapEvent
  {
    public sealed unsafe class Factory : TrapHookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.AIActionExamineTrap> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint> pHook = &OnAIActionExamineTrap;
        Hook = HookService.RequestHook<Functions.CNWSCreature.AIActionExamineTrap>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static uint OnAIActionExamineTrap(void* pCreature, void* pNode)
      {
        OnTrapExamine eventData = HandleExistingTrapEvent<OnTrapExamine>(pCreature, pNode);
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
    /// <inheritdoc cref="Events.OnTrapExamine"/>
    public event Action<OnTrapExamine> OnTrapExamine
    {
      add => EventService.Subscribe<OnTrapExamine, OnTrapExamine.Factory>(this, value);
      remove => EventService.Unsubscribe<OnTrapExamine, OnTrapExamine.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnTrapExamine"/>
    public event Action<OnTrapExamine> OnTrapExamine
    {
      add => EventService.SubscribeAll<OnTrapExamine, OnTrapExamine.Factory>(value);
      remove => EventService.UnsubscribeAll<OnTrapExamine, OnTrapExamine.Factory>(value);
    }
  }
}
