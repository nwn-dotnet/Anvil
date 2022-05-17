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
    /// Triggered when the <see cref="NwCreature"/> is blocked by a <see cref="NwDoor"/>.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnBlockedByDoor)]
    public sealed class OnBlocked : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwDoor"/> that is blocking the <see cref="NwCreature"/>.
      /// </summary>
      public NwDoor BlockingDoor { get; } = NWScript.GetBlockingDoor().ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the blocked <see cref="NwCreature"/>.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.OBJECT_SELF.ToNwObject<NwCreature>()!;

      NwObject IEvent.Context => Creature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="CreatureEvents.OnBlocked"/>
    public event Action<CreatureEvents.OnBlocked> OnBlocked
    {
      add => EventService.Subscribe<CreatureEvents.OnBlocked, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnBlocked, GameEventFactory>(this, value);
    }
  }
}
