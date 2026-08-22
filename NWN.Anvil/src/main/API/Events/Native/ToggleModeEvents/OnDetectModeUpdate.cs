using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a creature enters or exits detect mode.
  /// </summary>
  public sealed class OnDetectModeUpdate : IEvent
  {
    /// <summary>
    /// Gets the creature whose detect mode is being updated.
    /// </summary>
    public NwCreature Creature { get; private init; } = null!;

    /// <summary>
    /// Gets the event type indicating whether detect mode is being entered or exited.
    /// </summary>
    public ToggleModeEventType EventType { get; private init; }

    /// <summary>
    /// Set to true to prevent entering or exiting detect mode, depending on <see cref="EventType"/>.
    /// </summary>
    public bool Prevent { get; set; }

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.SetDetectMode> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, byte, void> pHook = &OnSetDetectMode;
        Hook = HookService.RequestHook<Functions.CNWSCreature.SetDetectMode>(pHook, HookOrder.Early);
        return [Hook];
      }

      private static void HandleEnter(CNWSCreature creature, byte nDetectMode)
      {
        OnDetectModeUpdate eventData = ProcessEvent(EventCallbackType.Before, new OnDetectModeUpdate
        {
          Creature = creature.ToNwObject<NwCreature>()!,
          EventType = ToggleModeEventType.Enter,
        });

        if (!eventData.Prevent)
        {
          Hook.CallOriginal(creature, nDetectMode);
        }
        else
        {
          creature.ClearActivities(0);
        }

        ProcessEvent(EventCallbackType.After, eventData);
      }

      private static void HandleExit(CNWSCreature creature, byte nDetectMode)
      {
        OnDetectModeUpdate eventData = ProcessEvent(EventCallbackType.Before, new OnDetectModeUpdate
        {
          Creature = creature.ToNwObject<NwCreature>()!,
          EventType = ToggleModeEventType.Exit,
        });

        if (!eventData.Prevent)
        {
          Hook.CallOriginal(creature, nDetectMode);
        }
        else
        {
          creature.SetActivity(0, true.ToInt());
        }

        ProcessEvent(EventCallbackType.After, eventData);
      }

      [UnmanagedCallersOnly]
      private static void OnSetDetectMode(void* pCreature, byte nDetectMode)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        bool willBeDetecting = nDetectMode != 0;
        bool currentlyDetecting = creature.m_nDetectMode != 0;

        switch (currentlyDetecting)
        {
          case false when willBeDetecting:
            HandleEnter(creature, nDetectMode);
            break;
          case true when !willBeDetecting:
            HandleExit(creature, nDetectMode);
            break;
          default:
            Hook.CallOriginal(pCreature, nDetectMode);
            break;
        }
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnDetectModeUpdate"/>
    public event Action<OnDetectModeUpdate> OnDetectModeUpdate
    {
      add => EventService.Subscribe<OnDetectModeUpdate, OnDetectModeUpdate.Factory>(this, value);
      remove => EventService.Unsubscribe<OnDetectModeUpdate, OnDetectModeUpdate.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDetectModeUpdate"/>
    public event Action<OnDetectModeUpdate> OnDetectModeUpdate
    {
      add => EventService.SubscribeAll<OnDetectModeUpdate, OnDetectModeUpdate.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDetectModeUpdate, OnDetectModeUpdate.Factory>(value);
    }
  }
}
