using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for Merchant/Store objects.
  /// </summary>
  public static partial class StoreEvents
  {
    [GameEvent(EventScriptType.StoreOnClose)]
    public sealed class OnClose : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that last closed this <see cref="NwStore"/>.
      /// </summary>
      public NwCreature Creature { get; } = NWScript.GetLastClosedBy().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwStore"/> being closed.
      /// </summary>
      public NwStore Store { get; } = NWScript.OBJECT_SELF.ToNwObject<NwStore>();

      NwObject IEvent.Context => Store;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwStore
  {
    /// <inheritdoc cref="StoreEvents.OnClose"/>
    public event Action<StoreEvents.OnClose> OnClose
    {
      add => EventService.Subscribe<StoreEvents.OnClose, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<StoreEvents.OnClose, GameEventFactory>(this, value);
    }
  }
}
