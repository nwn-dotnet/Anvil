using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a door's open state is changed (open/closed/destroyed).
  /// </summary>
  public sealed class OnDoorSetOpenState : IEvent
  {
    /// <summary>
    /// The door that is being open/closed.
    /// </summary>
    public NwDoor Door { get; private init; } = null!;

    /// <summary>
    /// The new open state of the door.
    /// </summary>
    public DoorOpenState OpenState { get; set; }

    /// <summary>
    /// Gets or sets if the door state should not be changed.
    /// </summary>
    public bool PreventStateChange { get; set; }

    NwObject IEvent.Context => Door;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<SetOpenStateHook> Hook { get; set; } = null!;

      [NativeFunction("_ZN8CNWSDoor12SetOpenStateEh", "")]
      private delegate void SetOpenStateHook(void* pDoor, byte nOpenState);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, byte, void> pHook = &OnSetOpenState;
        Hook = HookService.RequestHook<SetOpenStateHook>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnSetOpenState(void* pDoor, byte nOpenState)
      {
        NwDoor? door = CNWSDoor.FromPointer(pDoor).ToNwObject<NwDoor>();
        if (door == null)
        {
          Hook.CallOriginal(pDoor, nOpenState);
          return;
        }

        OnDoorSetOpenState eventData = ProcessEvent(EventCallbackType.Before, new OnDoorSetOpenState
        {
          Door = door,
          OpenState = (DoorOpenState)nOpenState,
        });

        if (!eventData.PreventStateChange)
        {
          Hook.CallOriginal(pDoor, (byte)eventData.OpenState);
        }

        ProcessEvent(EventCallbackType.After, eventData);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="Events.OnDoorSetOpenState"/>
    public event Action<OnDoorSetOpenState> OnDoorSetOpenState
    {
      add => EventService.Subscribe<OnDoorSetOpenState, OnDoorSetOpenState.Factory>(this, value);
      remove => EventService.Unsubscribe<OnDoorSetOpenState, OnDoorSetOpenState.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDoorSetOpenState"/>
    public event Action<OnDoorSetOpenState> OnDoorSetOpenState
    {
      add => EventService.SubscribeAll<OnDoorSetOpenState, OnDoorSetOpenState.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDoorSetOpenState, OnDoorSetOpenState.Factory>(value);
    }
  }
}
