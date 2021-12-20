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
    [GameEvent(EventScriptType.EncounterOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwEncounter"/> that was entered.
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
    /// <inheritdoc cref="EncounterEvents.OnEnter"/>
    public event Action<EncounterEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<EncounterEvents.OnEnter, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<EncounterEvents.OnEnter, GameEventFactory>(this, value);
    }
  }
}
