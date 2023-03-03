using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific trigger.
  /// </summary>
  public static partial class TriggerEvents
  {
    [GameEvent(EventScriptType.TriggerOnDisarmed)]
    public sealed class OnDisarmed : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> who disarmed this trigger.
      /// </summary>
      public NwCreature? DisarmedBy { get; } = NWScript.GetLastDisarmed().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that was disarmed.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>()!;

      NwObject IEvent.Context => Trigger;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwTrigger
  {
    /// <inheritdoc cref="TriggerEvents.OnDisarmed"/>
    public event Action<TriggerEvents.OnDisarmed> OnDisarmed
    {
      add => EventService.Subscribe<TriggerEvents.OnDisarmed, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnDisarmed, GameEventFactory>(this, value);
    }
  }
}
