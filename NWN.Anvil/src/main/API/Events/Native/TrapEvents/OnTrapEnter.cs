using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a game object enters a trap trigger.
  /// </summary>
  public sealed class OnTrapEnter : IEvent
  {
    /// <summary>
    /// Gets the object that entered the trap trigger.
    /// </summary>
    public NwGameObject EnteredObject { get; private init; } = null!;

    /// <summary>
    /// Gets the trap trigger.
    /// </summary>
    public NwTrigger Trigger { get; private init; } = null!;

    /// <summary>
    /// Gets if this trap was force set.
    /// </summary>
    public bool ForceSet { get; private init; }

    /// <summary>
    /// Gets or sets if the trap trigger event should be skipped.
    /// </summary>
    public bool Skip { get; set; }

    NwObject IEvent.Context => Trigger;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSTrigger.OnEnterTrap> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, void> pHook = &OnAIActionEnterTrap;
        Hook = HookService.RequestHook<Functions.CNWSTrigger.OnEnterTrap>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static void OnAIActionEnterTrap(void* pTrigger, int bForceSet)
      {
        NwTrigger trigger = CNWSTrigger.FromPointer(pTrigger).ToNwObject<NwTrigger>()!;
        OnTrapEnter eventData = ProcessEvent(EventCallbackType.Before, new OnTrapEnter
        {
          Trigger = trigger,
          EnteredObject = trigger.Trigger.m_oidLastEntered.ToNwObject<NwGameObject>()!,
          ForceSet = bForceSet.ToBool(),
        });

        if (!eventData.Skip)
        {
          Hook.CallOriginal(pTrigger, bForceSet);
        }

        ProcessEvent(EventCallbackType.After, eventData);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwTrigger
  {
    /// <inheritdoc cref="Events.OnTrapEnter"/>
    public event Action<OnTrapEnter> OnTrapEnter
    {
      add => EventService.Subscribe<OnTrapEnter, OnTrapEnter.Factory>(this, value);
      remove => EventService.Unsubscribe<OnTrapEnter, OnTrapEnter.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnTrapEnter"/>
    public event Action<OnTrapEnter> OnTrapEnter
    {
      add => EventService.SubscribeAll<OnTrapEnter, OnTrapEnter.Factory>(value);
      remove => EventService.UnsubscribeAll<OnTrapEnter, OnTrapEnter.Factory>(value);
    }
  }
}
