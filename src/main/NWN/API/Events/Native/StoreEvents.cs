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
    public sealed class OnOpen : NativeEvent<NwStore, OnOpen>
    {
      /// <summary>
      /// Gets the store being open.
      /// </summary>
      public NwStore Store { get; private set; }

      /// <summary>
      /// Gets the player that last opened this store.
      /// </summary>
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwStore objSelf)
      {
        Store = objSelf;
        Player = NWScript.GetLastOpenedBy().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.StoreOnClose)]
    public sealed class OnClose : NativeEvent<NwStore, OnClose>
    {
      /// <summary>
      /// Gets the store being closed.
      /// </summary>
      public NwStore Store { get; private set; }

      /// <summary>
      /// Gets the player that last closed this store.
      /// </summary>
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwStore objSelf)
      {
        Store = objSelf;
        Player = NWScript.GetLastClosedBy().ToNwObject<NwPlayer>();
      }
    }
  }
}
