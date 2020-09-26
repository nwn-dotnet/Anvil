using NWN.API;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public class BarterEvents
  {
    [NWNXEvent("NWNX_ON_BARTER_START_BEFORE")]
    public class OnBarterStartBefore : EventSkippable<OnBarterStartBefore>
    {
      public NwPlayer Player { get; private set; }
      public NwPlayer Target { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
        Target = NWScript.StringToObject(EventsPlugin.GetEventData("BARTER_TARGET")).ToNwObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_BARTER_START_AFTER")]
    public class OnBarterStartAfter : EventSkippable<OnBarterStartAfter>
    {
      public NwPlayer Player { get; private set; }
      public NwPlayer Target { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
        Target = NWScript.StringToObject(EventsPlugin.GetEventData("BARTER_TARGET")).ToNwObject<NwPlayer>();
      }
    }
  }
}
