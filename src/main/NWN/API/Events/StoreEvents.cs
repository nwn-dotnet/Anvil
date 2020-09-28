using NWN.API.Constants;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for Merchant/Store objects.
  /// </summary>
  public static class StoreEvents
  {
    [ScriptEvent(EventScriptType.StoreOnOpen)]
    public sealed class OnOpen : Event<NwStore, OnOpen>
    {
      public NwStore Store { get; private set; }

      protected override void PrepareEvent(NwStore objSelf)
      {
        Store = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.StoreOnClose)]
    public sealed class OnClose : Event<NwStore, OnClose>
    {
      public NwStore Store { get; private set; }

      protected override void PrepareEvent(NwStore objSelf)
      {
        Store = objSelf;
      }
    }
  }
}
