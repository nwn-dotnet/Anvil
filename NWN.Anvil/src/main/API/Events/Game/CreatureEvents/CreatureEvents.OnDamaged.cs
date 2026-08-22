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
    /// Triggered when the creature takes damage from a game object.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnDamaged)]
    public sealed class OnDamaged : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that is taking damage.
      /// </summary>
      public NwCreature Creature { get; }

      /// <summary>
      /// Gets the total damage dealt to the creature.
      /// </summary>
      public int DamageAmount { get; }

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that damaged <see cref="NwCreature"/>.
      /// </summary>
      public NwGameObject Damager { get; }

      /// <summary>
      /// Gets the damage dealt to the creature by type.
      /// </summary>
      public int GetDamageDealtByType(DamageType damageType)
      {
        return NWScript.GetDamageDealtByType((int)damageType);
      }

      NwObject IEvent.Context => Creature;

      public OnDamaged()
      {
        uint objSelf = NWScript.OBJECT_SELF;
        Creature = objSelf.ToNwObject<NwCreature>()!;
        DamageAmount = NWScript.GetTotalDamageDealt();
        Damager = NWScript.GetLastDamager(objSelf).ToNwObject<NwGameObject>()!;
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
