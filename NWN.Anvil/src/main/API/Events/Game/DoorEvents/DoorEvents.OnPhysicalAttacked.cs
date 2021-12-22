using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for door objects.
  /// </summary>
  public static partial class DoorEvents
  {
    [GameEvent(EventScriptType.DoorOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that attacked the <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature Attacker { get; } = NWScript.GetLastAttacker().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="SpecialAttack"/> used to damage <see cref="NwDoor"/>.
      /// </summary>
      public SpecialAttack AttackType { get; } = (SpecialAttack)NWScript.GetLastAttackType();

      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was physically attacked.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;

      /// <summary>
      /// Gets the <see cref="ActionMode"/> used to damage <see cref="NwDoor"/>.
      /// </summary>
      public ActionMode AttackMode(NwCreature attacker)
      {
        return (ActionMode)NWScript.GetLastAttackMode(attacker);
      }

      /// <summary>
      /// Gets the <see cref="NwItem"/> used to damage <see cref="NwDoor"/>.
      /// </summary>
      public NwItem WeaponUsed(NwCreature attacker)
      {
        return NWScript.GetLastWeaponUsed(attacker).ToNwObject<NwItem>();
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnPhysicalAttacked"/>
    public event Action<DoorEvents.OnPhysicalAttacked> OnPhysicalAttacked
    {
      add => EventService.Subscribe<DoorEvents.OnPhysicalAttacked, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnPhysicalAttacked, GameEventFactory>(this, value);
    }
  }
}
