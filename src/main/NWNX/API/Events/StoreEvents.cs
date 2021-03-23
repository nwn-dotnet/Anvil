using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class StoreEvents
  {
    [NWNXEvent("NWNX_ON_STORE_REQUEST_BUY_BEFORE")]
    public sealed class OnStoreRequestBuyBefore : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwItem Item { get; } = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();

      public NwStore Store { get; } = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();

      public int Price { get; } = EventsPlugin.GetEventData("PRICE").ParseInt();

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_STORE_REQUEST_BUY_AFTER")]
    public sealed class OnStoreRequestBuyAfter : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwItem Item { get; } = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();

      public NwStore Store { get; } = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();

      public int Price { get; } = EventsPlugin.GetEventData("PRICE").ParseInt();

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_STORE_REQUEST_SELL_BEFORE")]
    public sealed class OnStoreRequestSellBefore : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwItem Item { get; } = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();

      public NwStore Store { get; } = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();

      public int Price { get; } = EventsPlugin.GetEventData("PRICE").ParseInt();

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_STORE_REQUEST_SELL_AFTER")]
    public sealed class OnStoreRequestSellAfter : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public NwItem Item { get; } = EventsPlugin.GetEventData("ITEM").ParseObject<NwItem>();

      public NwStore Store { get; } = EventsPlugin.GetEventData("STORE").ParseObject<NwStore>();

      public int Price { get; } = EventsPlugin.GetEventData("PRICE").ParseInt();

      NwObject IEvent.Context => Player;
    }
  }
}
