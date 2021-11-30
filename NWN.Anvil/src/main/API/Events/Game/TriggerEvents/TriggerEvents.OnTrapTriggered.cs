using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for drawn, world-placed triggers.
  /// </summary>
  public static partial class TriggerEvents
  {
    [GameEvent(EventScriptType.TriggerOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that was triggered.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that triggered this <see cref="NwTrigger"/>.
      /// </summary>
      public NwGameObject TriggeredBy { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context
      {
        get => Trigger;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwTrigger
  {
    /// <inheritdoc cref="TriggerEvents.OnTrapTriggered"/>
    public event Action<TriggerEvents.OnTrapTriggered> OnTrapTriggered
    {
      add => EventService.Subscribe<TriggerEvents.OnTrapTriggered, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnTrapTriggered, GameEventFactory>(this, value);
    }
  }
}
