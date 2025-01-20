using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific store.
  /// </summary>
  public static partial class StoreEvents
  {
    [GameEvent(EventScriptType.StoreOnClose)]
    public sealed class OnClose : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that last closed this <see cref="NwStore"/>.
      /// </summary>
      public NwCreature Creature { get; }

      /// <summary>
      /// Gets the <see cref="NwStore"/> being closed.
      /// </summary>
      public NwStore Store { get; }

      NwObject IEvent.Context => Store;

      public OnClose()
      {
        Store = NWScript.OBJECT_SELF.ToNwObject<NwStore>()!;
        Creature = NWScript.GetLastClosedBy(Store).ToNwObject<NwCreature>()!;
      }
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
