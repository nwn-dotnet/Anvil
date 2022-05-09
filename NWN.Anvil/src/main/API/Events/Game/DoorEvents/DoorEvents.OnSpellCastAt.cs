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
    [GameEvent(EventScriptType.DoorOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the caster of this <see cref="Spell"/> (<see cref="NwCreature"/>, <see cref="NwPlaceable"/>, <see cref="NwDoor"/>). Returns null from an <see cref="NwAreaOfEffect"/>.
      /// </summary>
      public NwGameObject Caster { get; } = NWScript.GetLastSpellCaster().ToNwObject<NwGameObject>()!;

      /// <summary>
      /// Gets the <see cref="NwDoor"/> targeted by this <see cref="Spell"/>.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets a value indicating whether this spell is considered harmful.
      /// </summary>
      public bool Harmful { get; } = NWScript.GetLastSpellHarmful().ToBool();

      /// <summary>
      /// Gets the <see cref="Spell"/> that was cast.
      /// </summary>
      public NwSpell Spell { get; } = NwSpell.FromSpellId(NWScript.GetLastSpell());

      NwObject? IEvent.Context => Door;

      public static void Signal(NwObject caster, NwDoor target, NwSpell spell, bool harmful = true)
      {
        Event nwEvent = NWScript.EventSpellCastAt(caster, spell.Id, harmful.ToInt())!;
        NWScript.SignalEvent(target, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnSpellCastAt"/>
    public event Action<DoorEvents.OnSpellCastAt> OnSpellCastAt
    {
      add => EventService.Subscribe<DoorEvents.OnSpellCastAt, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnSpellCastAt, GameEventFactory>(this, value);
    }
  }
}
