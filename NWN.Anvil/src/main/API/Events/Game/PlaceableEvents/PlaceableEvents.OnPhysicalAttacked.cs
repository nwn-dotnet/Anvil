using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific placeable.
  /// </summary>
  public static partial class PlaceableEvents
  {
    /// <summary>
    /// Called when <see cref="NwPlaceable"/> has been physically attacked.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      public OnPhysicalAttacked()
      {
        Placeable = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>()!;
        Attacker = NWScript.GetLastAttacker(Placeable).ToNwObject<NwCreature>();
        AttackType = (SpecialAttack)NWScript.GetLastAttackType(Attacker);
      }

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that attacked the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature? Attacker { get; }

      /// <summary>
      /// Gets the <see cref="SpecialAttack"/> used to damage <see cref="NwPlaceable"/>.
      /// </summary>
      public SpecialAttack AttackType { get; }

      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was physically attacked.
      /// </summary>
      public NwPlaceable Placeable { get; }

      NwObject IEvent.Context => Placeable;

      /// <summary>
      /// Gets the <see cref="ActionMode"/> used to damage <see cref="NwPlaceable"/>.
      /// </summary>
      public ActionMode AttackMode(NwCreature attacker)
      {
        return (ActionMode)NWScript.GetLastAttackMode(attacker);
      }

      /// <summary>
      /// Gets the <see cref="NwItem"/> used to damage <see cref="NwPlaceable"/>.
      /// </summary>
      public NwItem? WeaponUsed(NwCreature attacker)
      {
        return NWScript.GetLastWeaponUsed(attacker).ToNwObject<NwItem>();
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnPhysicalAttacked"/>
    public event Action<PlaceableEvents.OnPhysicalAttacked> OnPhysicalAttacked
    {
      add => EventService.Subscribe<PlaceableEvents.OnPhysicalAttacked, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnPhysicalAttacked, GameEventFactory>(this, value);
    }
  }
}
