using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using NWN.Native.API;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when an object enters a trigger.
  /// </summary>
  public sealed class OnTriggerEnter : IEvent
  {
    /// <summary>
    /// Gets the object that entered the trigger.
    /// </summary>
    public NwGameObject EnteredObject { get; private init; } = null!;

    /// <summary>
    /// Gets if the trigger is considered a trap.
    /// </summary>
    public bool IsTrap { get; private init; }

    /// <summary>
    /// Gets if the trigger was force set.
    /// </summary>
    public bool IsTrapForceSet { get; private init; }

    /// <summary>
    /// Gets or sets whether this trigger should fire.
    /// </summary>
    public bool Skip { get; set; }

    /// <summary>
    /// Gets the trigger that was entered.
    /// </summary>
    public NwTrigger Trigger { get; private init; } = null!;

    NwObject IEvent.Context => Trigger;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<TriggerEventHandlerHook> Hook { get; set; } = null!;

      private delegate void TriggerEventHandlerHook(void* pTrigger, uint nEventId, uint nCallerObjectId, void* pScript, uint nCalendarDay, uint nTimeOfDay);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, uint, void*, uint, uint, void> pHook = &OnTriggerEventHandler;
        Hook = HookService.RequestHook<TriggerEventHandlerHook>(pHook, FunctionsLinux._ZN11CNWSTrigger12EventHandlerEjjPvjj, HookOrder.Late);
        return new IDisposable[] { Hook };
      }

      private static bool HandleEnter(CNWSTrigger trigger, CScriptEvent scriptEvent)
      {
        OnTriggerEnter eventData = ProcessEvent(new OnTriggerEnter
        {
          Trigger = trigger.ToNwObject<NwTrigger>()!,
          EnteredObject = scriptEvent.GetObjectID(0).ToNwObject<NwGameObject>()!,
          IsTrap = trigger.m_bTrap.ToBool(),
          IsTrapForceSet = scriptEvent.GetInteger(0).ToBool(),
        });

        return eventData.Skip;
      }

      [UnmanagedCallersOnly]
      private static void OnTriggerEventHandler(void* pTrigger, uint nEventId, uint nCallerObjectId, void* pScript, uint nCalendarDay, uint nTimeOfDay)
      {
        if (nEventId == (uint)AIMasterEvent.SignalEvent)
        {
          CNWSTrigger trigger = CNWSTrigger.FromPointer(pTrigger);
          CScriptEvent scriptEvent = CScriptEvent.FromPointer(pScript);

          switch ((ScriptEvent)scriptEvent.m_nType)
          {
            case ScriptEvent.OnObjectEnter:
              if (HandleEnter(trigger, scriptEvent))
              {
                scriptEvent = CScriptEvent.FromPointer(pScript, true);
                scriptEvent.Dispose();
                return;
              }

              break;
          }
        }

        Hook.CallOriginal(pTrigger, nEventId, nCallerObjectId, pScript, nCalendarDay, nTimeOfDay);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwTrigger
  {
    /// <inheritdoc cref="Events.OnTriggerEnter"/>
    public event Action<OnTriggerEnter> OnDetectModeUpdate
    {
      add => EventService.Subscribe<OnTriggerEnter, OnTriggerEnter.Factory>(this, value);
      remove => EventService.Unsubscribe<OnTriggerEnter, OnTriggerEnter.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnTriggerEnter"/>
    public event Action<OnTriggerEnter> OnTriggerEnter
    {
      add => EventService.SubscribeAll<OnTriggerEnter, OnTriggerEnter.Factory>(value);
      remove => EventService.UnsubscribeAll<OnTriggerEnter, OnTriggerEnter.Factory>(value);
    }
  }
}
