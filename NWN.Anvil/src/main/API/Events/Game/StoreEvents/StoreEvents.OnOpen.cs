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
    [GameEvent(EventScriptType.StoreOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that last opened this store.
      /// </summary>
      public NwPlayer Player { get; }

      /// <summary>
      /// Gets the <see cref="NwStore"/> being open.
      /// </summary>
      public NwStore Store { get; }

      NwObject IEvent.Context => Store;

      public OnOpen()
      {
        Store = NWScript.OBJECT_SELF.ToNwObject<NwStore>()!;
        Player = NWScript.GetLastOpenedBy(Store).ToNwPlayer()!;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwStore
  {
    /// <inheritdoc cref="StoreEvents.OnOpen"/>
    public event Action<StoreEvents.OnOpen> OnOpen
    {
      add => EventService.Subscribe<StoreEvents.OnOpen, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<StoreEvents.OnOpen, GameEventFactory>(this, value);
    }
  }
}
