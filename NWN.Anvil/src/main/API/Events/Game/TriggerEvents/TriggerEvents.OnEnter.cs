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
    [GameEvent(EventScriptType.TriggerOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that entered this <see cref="NwTrigger"/>.
      /// </summary>
      public NwGameObject EnteringObject { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>()!;

      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that was entered.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>()!;

      NwObject? IEvent.Context => Trigger;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwTrigger
  {
    /// <inheritdoc cref="TriggerEvents.OnEnter"/>
    public event Action<TriggerEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<TriggerEvents.OnEnter, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnEnter, GameEventFactory>(this, value);
    }
  }
}
