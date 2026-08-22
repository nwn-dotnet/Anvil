using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific area.
  /// </summary>
  public static partial class AreaEvents
  {
    /// <summary>
    /// Triggered for an area when the associated <see cref="Signal"/> method is called.
    /// </summary>
    /// <seealso cref="Signal"/>
    [GameEvent(EventScriptType.AreaOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the area where the event was triggered.
      /// </summary>
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>()!;

      /// <summary>
      /// Gets the user-defined event number that was included in the event trigger.
      /// </summary>
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      /// <inheritdoc/>
      NwObject IEvent.Context => Area;

      /// <summary>
      /// Creates a user-defined event and signals it to the specified area.
      /// </summary>
      /// <param name="area">The target area that will receive the event.</param>
      /// <param name="eventId">The user-defined event number to signal.</param>
      public static void Signal(NwArea area, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId)!;
        NWScript.SignalEvent(area, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwArea
  {
    /// <inheritdoc cref="AreaEvents.OnUserDefined"/>
    public event Action<AreaEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<AreaEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
