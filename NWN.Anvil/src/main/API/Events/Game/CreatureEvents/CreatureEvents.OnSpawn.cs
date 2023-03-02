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
    /// Triggered by <see cref="NwCreature"/> upon spawning into the game.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnSpawnIn)]
    public sealed class OnSpawn : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that has spawned into the game.
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
    /// <inheritdoc cref="CreatureEvents.OnSpawn"/>
    public event Action<CreatureEvents.OnSpawn> OnSpawn
    {
      add => EventService.Subscribe<CreatureEvents.OnSpawn, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnSpawn, GameEventFactory>(this, value);
    }
  }
}
