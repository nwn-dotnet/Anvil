using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for Merchant/Store objects.
  /// </summary>
  public static class StoreEvents
  {
    [GameEvent(EventScriptType.StoreOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the store being open.
      /// </summary>
      public NwStore Store { get; } = NWScript.OBJECT_SELF.ToNwObject<NwStore>();

      /// <summary>
      /// Gets the player that last opened this store.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastOpenedBy().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Store;
    }

    [GameEvent(EventScriptType.StoreOnClose)]
    public sealed class OnClose : IEvent
    {
      /// <summary>
      /// Gets the store being closed.
      /// </summary>
      public NwStore Store { get; } = NWScript.OBJECT_SELF.ToNwObject<NwStore>();

      /// <summary>
      /// Gets the player that last closed this store.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.GetLastClosedBy().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Store;
    }
  }
}
