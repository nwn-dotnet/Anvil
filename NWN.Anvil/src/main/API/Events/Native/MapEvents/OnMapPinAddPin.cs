using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using NWN.Native.API;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a player is about to place a map pin.
  /// </summary>
  public sealed class OnMapPinAddPin : IEvent
  {
    /// <summary>
    /// Gets the note that was set on the map pin.
    /// </summary>
    public string Note { get; private init; } = null!;

    /// <summary>
    /// Gets the player that placed the pin.
    /// </summary>
    public NwPlayer Player { get; private init; } = null!;

    /// <summary>
    /// Gets the position that the pin was placed at.
    /// </summary>
    public Vector3 Position { get; private init; }

    /// <summary>
    /// Gets or sets if this pin add event should be prevented.
    /// </summary>
    public bool PreventPinAdd { get; set; }

    NwObject? IEvent.Context => Player.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.HandlePlayerToServerMapPinSetMapPinAt> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int> pHook = &OnHandleMapPinSetMapPinAtMessage;
        Hook = HookService.RequestHook<Functions.CNWSMessage.HandlePlayerToServerMapPinSetMapPinAt>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnHandleMapPinSetMapPinAtMessage(void* pMessage, void* pPlayer)
      {
        CNWSMessage message = CNWSMessage.FromPointer(pMessage);
        CNWSPlayer player = CNWSPlayer.FromPointer(pPlayer);

        if (player == null)
        {
          return Hook.CallOriginal(pMessage, pPlayer);
        }

        // Coordinates
        int offset = 0;
        float x = message.PeekMessage<float>(offset);
        offset += sizeof(float);
        float y = message.PeekMessage<float>(offset);
        offset += sizeof(float);
        float z = message.PeekMessage<float>(offset);
        offset += sizeof(float);

        // Note
        string note = message.PeekMessageString(offset);

        OnMapPinAddPin eventData = ProcessEvent(EventCallbackType.Before, new OnMapPinAddPin
        {
          Player = player.ToNwPlayer()!,
          Position = new Vector3(x, y, z),
          Note = note,
        });

        int retVal = eventData.PreventPinAdd ? false.ToInt() : Hook.CallOriginal(pMessage, pPlayer);
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
    /// <inheritdoc cref="Events.OnMapPinAddPin"/>
    public event Action<OnMapPinAddPin> OnMapPinAddPin
    {
      add => EventService.Subscribe<OnMapPinAddPin, OnMapPinAddPin.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnMapPinAddPin, OnMapPinAddPin.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnMapPinAddPin"/>
    public event Action<OnMapPinAddPin> OnMapPinAddPin
    {
      add => EventService.SubscribeAll<OnMapPinAddPin, OnMapPinAddPin.Factory>(value);
      remove => EventService.UnsubscribeAll<OnMapPinAddPin, OnMapPinAddPin.Factory>(value);
    }
  }
}
