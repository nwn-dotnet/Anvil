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
    [GameEvent(EventScriptType.StoreOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlayer"/> that last opened this store.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastOpenedBy().ToNwPlayer();

      /// <summary>
      /// Gets the <see cref="NwStore"/> being open.
      /// </summary>
      public NwStore Store { get; } = NWScript.OBJECT_SELF.ToNwObject<NwStore>();

      NwObject IEvent.Context
      {
        get => Store;
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
