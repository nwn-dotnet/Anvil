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
    /// Triggered by <see cref="NwCreature"/> when taken damage from <see cref="NwGameObject"/>.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnDamaged)]
    public sealed class OnDamaged : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that is taking damage.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the amount of damage done by <see cref="NwGameObject"/> to <see cref="NwCreature"/>.
      /// </summary>
      public int DamageAmount { get; } = NWScript.GetTotalDamageDealt();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that damaged <see cref="NwCreature"/>.
      /// </summary>
      public NwGameObject Damager { get; } = NWScript.GetLastDamager().ToNwObject<NwGameObject>();

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
    /// <inheritdoc cref="CreatureEvents.OnDamaged"/>
    public event Action<CreatureEvents.OnDamaged> OnDamaged
    {
      add => EventService.Subscribe<CreatureEvents.OnDamaged, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnDamaged, GameEventFactory>(this, value);
    }
  }
}
