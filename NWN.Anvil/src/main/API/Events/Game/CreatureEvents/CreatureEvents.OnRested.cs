using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific creature.
  /// </summary>
  public static partial class CreatureEvents
  {
    /// <summary>
    /// Triggered by <see cref="NwCreature"/> when resting.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnRested)]
    public sealed class OnRested : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> is resting.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>()!;

      NwObject IEvent.Context => Creature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="CreatureEvents.OnRested"/>
    public event Action<CreatureEvents.OnRested> OnRested
    {
      add => EventService.Subscribe<CreatureEvents.OnRested, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnRested, GameEventFactory>(this, value);
    }
  }
}
