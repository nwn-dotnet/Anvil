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
    /// Called when <see cref="NwCreature"/> has destroyed <see cref="NwPlaceable"/>.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnDeath)]
    public sealed class OnDeath : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was destroyed.
      /// </summary>
      public NwPlaceable KilledObject { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>()!;

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that destroyed the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwCreature? Killer { get; } = NWScript.GetLastKiller().ToNwObject<NwCreature>();

      NwObject? IEvent.Context => KilledObject;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnDeath"/>
    public event Action<PlaceableEvents.OnDeath> OnDeath
    {
      add => EventService.Subscribe<PlaceableEvents.OnDeath, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnDeath, GameEventFactory>(this, value);
    }
  }
}
