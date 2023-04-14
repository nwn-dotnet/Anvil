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
    /// Triggered by <see cref="NwCreature"/> when killed by <see cref="NwGameObject"/>.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnDeath)]
    public sealed class OnDeath : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that is killed.
      /// </summary>
      public NwCreature KilledCreature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>()!;

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that killed <see cref="NwCreature"/>.
      /// </summary>
      public NwObject? Killer { get; } = NWScript.GetLastKiller().ToNwObject();

      NwObject IEvent.Context => KilledCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="CreatureEvents.OnDeath"/>
    public event Action<CreatureEvents.OnDeath> OnDeath
    {
      add => EventService.Subscribe<CreatureEvents.OnDeath, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnDeath, GameEventFactory>(this, value);
    }
  }
}
