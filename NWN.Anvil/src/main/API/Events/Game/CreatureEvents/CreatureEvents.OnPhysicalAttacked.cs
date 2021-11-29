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
    /// Triggered by <see cref="NwCreature"/> when physically attacked by another <see cref="NwCreature"/>.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that physically attacked <see cref="NwCreature"/>.
      /// </summary>
      public NwCreature Attacker { get; } = NWScript.GetLastAttacker().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that was physically attacked.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      NwObject IEvent.Context
      {
        get => Creature;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="CreatureEvents.OnPhysicalAttacked"/>
    public event Action<CreatureEvents.OnPhysicalAttacked> OnPhysicalAttacked
    {
      add => EventService.Subscribe<CreatureEvents.OnPhysicalAttacked, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnPhysicalAttacked, GameEventFactory>(this, value);
    }
  }
}
