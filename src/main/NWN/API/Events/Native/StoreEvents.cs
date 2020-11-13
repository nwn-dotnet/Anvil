using NWN.API.Constants;

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
      public NwStore Store { get; private set; }

      protected override void PrepareEvent(NwStore objSelf)
      {
        Store = objSelf;
      }
    }

    [NativeEvent(EventScriptType.StoreOnClose)]
    public sealed class OnClose : NativeEvent<NwStore, OnClose>
    {
      public NwStore Store { get; private set; }

      protected override void PrepareEvent(NwStore objSelf)
      {
        Store = objSelf;
      }
    }
  }
}
