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
  /// Called when a player is about to change an existing map pin.
  /// </summary>
  public sealed class OnMapPinChangePin : IEvent
  {
    /// <summary>
    /// Gets the unique identifier for this map pin.
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    /// Gets the note that was set on the map pin.
    /// </summary>
    public string? Note { get; private init; }

    /// <summary>
    /// Gets the player that is changing the map pin.
    /// </summary>
    public NwPlayer Player { get; private init; } = null!;

    /// <summary>
    /// Gets the position that the pin was placed at.
    /// </summary>
    public Vector3 Position { get; private init; }

    /// <summary>
    /// Gets or sets if this pin change event should be prevented.
    /// </summary>
    public bool PreventPinChange { get; set; }

    NwObject? IEvent.Context => Player.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.HandlePlayerToServerMapPinChangePin> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int> pHook = &OnHandleMapPinChangePinMessage;
        Hook = HookService.RequestHook<Functions.CNWSMessage.HandlePlayerToServerMapPinChangePin>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnHandleMapPinChangePinMessage(void* pMessage, void* pPlayer)
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
        offset += note.Length + 4;

        // Id
        int id = message.PeekMessage<int>(offset);

        OnMapPinChangePin eventData = ProcessEvent(EventCallbackType.Before, new OnMapPinChangePin
        {
          Player = player.ToNwPlayer()!,
          Position = new Vector3(x, y, z),
          Note = note,
          Id = id,
        });

        int retVal = eventData.PreventPinChange ? false.ToInt() : Hook.CallOriginal(pMessage, pPlayer);
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
    /// <inheritdoc cref="Events.OnMapPinChangePin"/>
    public event Action<OnMapPinChangePin> OnMapPinChangePin
    {
      add => EventService.Subscribe<OnMapPinChangePin, OnMapPinChangePin.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnMapPinChangePin, OnMapPinChangePin.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnMapPinChangePin"/>
    public event Action<OnMapPinChangePin> OnMapPinChangePin
    {
      add => EventService.SubscribeAll<OnMapPinChangePin, OnMapPinChangePin.Factory>(value);
      remove => EventService.UnsubscribeAll<OnMapPinChangePin, OnMapPinChangePin.Factory>(value);
    }
  }
}
