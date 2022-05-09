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
    /// Triggered at the end of the <see cref="NwCreature"/> combat round.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnEndCombatRound)]
    public sealed class OnCombatRoundEnd : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> whose combat round is ending.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>()!;

      NwObject? IEvent.Context => Creature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="CreatureEvents.OnCombatRoundEnd"/>
    public event Action<CreatureEvents.OnCombatRoundEnd> OnCombatRoundEnd
    {
      add => EventService.Subscribe<CreatureEvents.OnCombatRoundEnd, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnCombatRoundEnd, GameEventFactory>(this, value);
    }
  }
}
