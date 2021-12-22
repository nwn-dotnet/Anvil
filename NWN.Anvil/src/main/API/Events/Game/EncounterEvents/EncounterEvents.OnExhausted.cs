using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for <see cref="NwEncounter"/> triggers.
  /// </summary>
  public static partial class EncounterEvents
  {
    [GameEvent(EventScriptType.EncounterOnEncounterExhausted)]
    public sealed class OnExhausted : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> that was exhausted.
      /// </summary>
      public NwEncounter Encounter { get; } = NWScript.OBJECT_SELF.ToNwObject<NwEncounter>();

      NwObject IEvent.Context => Encounter;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwEncounter
  {
    /// <inheritdoc cref="EncounterEvents.OnExhausted"/>
    public event Action<EncounterEvents.OnExhausted> OnExhausted
    {
      add => EventService.Subscribe<EncounterEvents.OnExhausted, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<EncounterEvents.OnExhausted, GameEventFactory>(this, value);
    }
  }
}
