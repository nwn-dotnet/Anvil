using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public class StoreEvents
  {
    [NWNXEvent("NWNX_ON_STORE_REQUEST_BUY_BEFORE")]
    public class OnStoreRequestBuyBefore : Event<OnStoreRequestBuyBefore>
    {
      public NwPlayer Player { get; private set; }

      public NwItem Item { get; private set; }

      public NwStore Store { get; private set; }

      public int Price { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Item = NWScript.StringToObject(EventsPlugin.GetEventData("STORE")).ToNwObject<NwItem>();
        Store = NWScript.StringToObject(EventsPlugin.GetEventData("STORE")).ToNwObject<NwStore>();
        Price = EventsPlugin.GetEventData("PRICE").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_STORE_REQUEST_BUY_AFTER")]
    public class OnStoreRequestBuyAfter : Event<OnStoreRequestBuyAfter>
    {
      public NwPlayer Player { get; private set; }

      public NwItem Item { get; private set; }

      public NwStore Store { get; private set; }

      public int Price { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Item = NWScript.StringToObject(EventsPlugin.GetEventData("STORE")).ToNwObject<NwItem>();
        Store = NWScript.StringToObject(EventsPlugin.GetEventData("STORE")).ToNwObject<NwStore>();
        Price = EventsPlugin.GetEventData("PRICE").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_STORE_REQUEST_SELL_BEFORE")]
    public class OnStoreRequestSellBefore : Event<OnStoreRequestSellBefore>
    {
      public NwPlayer Player { get; private set; }

      public NwItem Item { get; private set; }

      public NwStore Store { get; private set; }

      public int Price { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Item = NWScript.StringToObject(EventsPlugin.GetEventData("STORE")).ToNwObject<NwItem>();
        Store = NWScript.StringToObject(EventsPlugin.GetEventData("STORE")).ToNwObject<NwStore>();
        Price = EventsPlugin.GetEventData("PRICE").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_STORE_REQUEST_SELL_AFTER")]
    public class OnStoreRequestSellAfter : Event<OnStoreRequestSellAfter>
    {
      public NwPlayer Player { get; private set; }

      public NwItem Item { get; private set; }

      public NwStore Store { get; private set; }

      public int Price { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Item = NWScript.StringToObject(EventsPlugin.GetEventData("STORE")).ToNwObject<NwItem>();
        Store = NWScript.StringToObject(EventsPlugin.GetEventData("STORE")).ToNwObject<NwStore>();
        Price = EventsPlugin.GetEventData("PRICE").ParseInt();
      }
    }
  }
}
