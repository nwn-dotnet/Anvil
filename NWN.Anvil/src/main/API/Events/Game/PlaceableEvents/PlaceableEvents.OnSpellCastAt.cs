using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for <see cref="NwPlaceable"/>.
  /// </summary>
  public static partial class PlaceableEvents
  {
    /// <summary>
    /// Called when <see cref="Spell"/> has been casted on <see cref="NwPlaceable"/>.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwGameObject"/> who cast <see cref="Spell"/> (<see cref="NwCreature"/>, <see cref="NwPlaceable"/>, <see cref="NwDoor"/>). Returns null from an <see cref="NwAreaOfEffect"/>.
      /// </summary>
      public NwGameObject Caster { get; } = NWScript.GetLastSpellCaster().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets a value indicating whether this spell is considered harmful.
      /// </summary>
      public bool Harmful { get; } = NWScript.GetLastSpellHarmful().ToBool();

      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> targeted by this spell.
      /// </summary>
      public NwPlaceable Placeable { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="Spell"/> that was cast.
      /// </summary>
      public Spell Spell { get; } = (Spell)NWScript.GetLastSpell();

      NwObject IEvent.Context
      {
        get => Placeable;
      }

      public static void Signal(NwObject caster, NwPlaceable target, Spell spell, bool harmful = true)
      {
        Event nwEvent = NWScript.EventSpellCastAt(caster, (int)spell, harmful.ToInt());
        NWScript.SignalEvent(target, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnSpellCastAt"/>
    public event Action<PlaceableEvents.OnSpellCastAt> OnSpellCastAt
    {
      add => EventService.Subscribe<PlaceableEvents.OnSpellCastAt, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnSpellCastAt, GameEventFactory>(this, value);
    }
  }
}
