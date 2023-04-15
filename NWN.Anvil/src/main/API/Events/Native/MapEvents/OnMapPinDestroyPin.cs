using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using NWN.Native.API;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a player is about to delete an existing map pin.
  /// </summary>
  public sealed class OnMapPinDestroyPin : IEvent
  {
    /// <summary>
    /// Gets the unique identifier for the pin being destroyed.
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    /// Gets the player that is changing the map pin.
    /// </summary>
    public NwPlayer Player { get; private init; } = null!;

    /// <summary>
    /// Gets or sets if this pin destroy event should be prevented.
    /// </summary>
    public bool PreventPinDestroy { get; set; }

    NwObject? IEvent.Context => Player.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.HandlePlayerToServerMapPinDestroyMapPin> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int> pHook = &OnHandleMapPinDestroyMapPinMessage;
        Hook = HookService.RequestHook<Functions.CNWSMessage.HandlePlayerToServerMapPinDestroyMapPin>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnHandleMapPinDestroyMapPinMessage(void* pMessage, void* pPlayer)
      {
        CNWSMessage message = CNWSMessage.FromPointer(pMessage);
        CNWSPlayer player = CNWSPlayer.FromPointer(pPlayer);

        if (player == null)
        {
          return Hook.CallOriginal(pMessage, pPlayer);
        }

        // Id
        int id = message.PeekMessage<int>(0);

        OnMapPinDestroyPin eventData = ProcessEvent(EventCallbackType.Before, new OnMapPinDestroyPin
        {
          Player = player.ToNwPlayer()!,
          Id = id,
        });

        int retVal = eventData.PreventPinDestroy ? false.ToInt() : Hook.CallOriginal(pMessage, pPlayer);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnMapPinDestroyPin"/>
    public event Action<OnMapPinDestroyPin> OnMapPinDestroyPin
    {
      add => EventService.Subscribe<OnMapPinDestroyPin, OnMapPinDestroyPin.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnMapPinDestroyPin, OnMapPinDestroyPin.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnMapPinDestroyPin"/>
    public event Action<OnMapPinDestroyPin> OnMapPinDestroyPin
    {
      add => EventService.SubscribeAll<OnMapPinDestroyPin, OnMapPinDestroyPin.Factory>(value);
      remove => EventService.UnsubscribeAll<OnMapPinDestroyPin, OnMapPinDestroyPin.Factory>(value);
    }
  }
}
