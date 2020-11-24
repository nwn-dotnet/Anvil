using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class BarterEvents
  {
    [NWNXEvent("NWNX_ON_BARTER_START_BEFORE")]
    public class OnBarterStartBefore : NWNXEventSkippable<OnBarterStartBefore>
    {
      public NwPlayer Player { get; private set; }

      public NwPlayer Target { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Target = EventsPlugin.GetEventData("BARTER_TARGET").ParseObject<NwPlayer>();
      }
    }

    [NWNXEvent("NWNX_ON_BARTER_START_AFTER")]
    public class OnBarterStartAfter : NWNXEventSkippable<OnBarterStartAfter>
    {
      public NwPlayer Player { get; private set; }

      public NwPlayer Target { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer) objSelf;
        Target = EventsPlugin.GetEventData("BARTER_TARGET").ParseObject<NwPlayer>();
      }
    }
  }
}
