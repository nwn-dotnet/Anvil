using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public class StoreEvents
  {
    [NWNXEvent("NWNX_ON_STORE_REQUEST_BUY_BEFORE")]
    public class OnStoreRequestBuyBefore : NWNXEvent<OnStoreRequestBuyBefore>
    {
      public NwPlayer Player { get; private set; }

      public NwItem Item { get; private set; }

      public NwStore Store { get; private set; }

      public int Price { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Item = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();
        Store = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();
        Price = EventsPlugin.GetEventData("PRICE").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_STORE_REQUEST_BUY_AFTER")]
    public class OnStoreRequestBuyAfter : NWNXEvent<OnStoreRequestBuyAfter>
    {
      public NwPlayer Player { get; private set; }

      public NwItem Item { get; private set; }

      public NwStore Store { get; private set; }

      public int Price { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Item = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();
        Store = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();
        Price = EventsPlugin.GetEventData("PRICE").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_STORE_REQUEST_SELL_BEFORE")]
    public class OnStoreRequestSellBefore : NWNXEvent<OnStoreRequestSellBefore>
    {
      public NwPlayer Player { get; private set; }

      public NwItem Item { get; private set; }

      public NwStore Store { get; private set; }

      public int Price { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Item = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();
        Store = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();
        Price = EventsPlugin.GetEventData("PRICE").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_STORE_REQUEST_SELL_AFTER")]
    public class OnStoreRequestSellAfter : NWNXEvent<OnStoreRequestSellAfter>
    {
      public NwPlayer Player { get; private set; }

      public NwItem Item { get; private set; }

      public NwStore Store { get; private set; }

      public int Price { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Item = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();
        Store = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();
        Price = EventsPlugin.GetEventData("PRICE").ParseInt();
      }
    }
  }
}
