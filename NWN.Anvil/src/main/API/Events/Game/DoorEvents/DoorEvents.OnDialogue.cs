using System;
using Anvil.API.Events;
using NWN.Core;

#pragma warning disable CS0618

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for door objects.
  /// </summary>
  public static partial class DoorEvents
  {
    [Obsolete("Use OnConversation instead.")]
    [GameEvent(EventScriptType.DoorOnDialogue)]
    public sealed class OnDialogue : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that is in dialog.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the listen pattern that matched the message sent to this door.
      /// </summary>
      public int ListenPattern { get; } = NWScript.GetListenPatternNumber();

      NwObject IEvent.Context => Door;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnDialogue"/>
    [Obsolete("Use OnConversation instead.")]
    public event Action<DoorEvents.OnDialogue> OnDialogue
    {
      add => EventService.Subscribe<DoorEvents.OnDialogue, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnDialogue, GameEventFactory>(this, value);
    }
  }
}
