using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for <see cref="NwCreature"/>.
  /// </summary>
  public static partial class CreatureEvents
  {
    /// <summary>
    /// Called at a regular interval (approx. 6 seconds).
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> associated with the heartbeat event.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>()!;

      NwObject? IEvent.Context => Creature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="CreatureEvents.OnHeartbeat"/>
    public event Action<CreatureEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<CreatureEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnHeartbeat, GameEventFactory>(this, value);
    }
  }
}
