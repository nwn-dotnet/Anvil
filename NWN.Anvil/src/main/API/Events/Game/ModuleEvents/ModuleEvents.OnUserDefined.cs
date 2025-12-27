using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Global module events.
  /// </summary>
  public static partial class ModuleEvents
  {
    /// <summary>
    /// Triggered for the module when the associated <see cref="Signal"/> method is called.
    /// </summary>
    /// <seealso cref="Signal"/>
    [GameEvent(EventScriptType.ModuleOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      /// <summary>
      /// Gets the specific event number used to trigger this user-defined event.
      /// </summary>
      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject? IEvent.Context => null;

      /// <summary>
      /// Signals a user-defined event to the module.
      /// </summary>
      /// <param name="eventId">The user-defined event number.</param>
      public static void Signal(int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId)!;
        NWScript.SignalEvent(NwModule.Instance, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnUserDefined"/>
    public event Action<ModuleEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.SubscribeAll<ModuleEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnUserDefined, GameEventFactory>(value);
    }
  }
}
