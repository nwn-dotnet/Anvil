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
    [NativeEvent(EventScriptType.StoreOnOpen)]
    public sealed class OnOpen : IEvent
    {
      /// <summary>
      /// Gets the store being open.
      /// </summary>
      public NwStore Store { get; private set; }

      /// <summary>
      /// Gets the player that last opened this store.
      /// </summary>
      public NwPlayer Player { get; private set; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Store;

      public OnOpen()
      {
        Store = NWScript.OBJECT_SELF.ToNwObject<NwStore>();
        Player = NWScript.GetLastOpenedBy().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.StoreOnClose)]
    public sealed class OnClose : IEvent
    {
      /// <summary>
      /// Gets the store being closed.
      /// </summary>
      public NwStore Store { get; private set; }

      /// <summary>
      /// Gets the player that last closed this store.
      /// </summary>
      public NwPlayer Player { get; private set; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Store;

      public OnClose()
      {
        Store = NWScript.OBJECT_SELF.ToNwObject<NwStore>();
        Player = NWScript.GetLastClosedBy().ToNwObject<NwPlayer>();
      }
    }
  }
}
