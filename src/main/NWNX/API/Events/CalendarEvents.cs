using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public class CalendarEvents
  {
    [NWNXEvent("NWNX_ON_CALENDAR_HOUR")]
    public class OnCalendarHour : NWNXEvent<OnCalendarHour>
    {
      public NwModule Module { get; private set; }

      public int Old { get; private set; }

      public int New { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Module = (NwModule) objSelf;
        Old = EventsPlugin.GetEventData("OLD").ParseInt();
        New = EventsPlugin.GetEventData("NEW").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_CALENDAR_DAY")]
    public class OnCalendarDay : NWNXEvent<OnCalendarDay>
    {
      public NwModule Module { get; private set; }

      public int Old { get; private set; }

      public int New { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Module = (NwModule) objSelf;
        Old = EventsPlugin.GetEventData("OLD").ParseInt();
        New = EventsPlugin.GetEventData("NEW").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_CALENDAR_MONTH")]
    public class OnCalendarMonth : NWNXEvent<OnCalendarMonth>
    {
      public NwModule Module { get; private set; }

      public int Old { get; private set; }

      public int New { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Module = (NwModule) objSelf;
        Old = EventsPlugin.GetEventData("OLD").ParseInt();
        New = EventsPlugin.GetEventData("NEW").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_CALENDAR_YEAR")]
    public class OnCalendarYear : NWNXEvent<OnCalendarYear>
    {
      public NwModule Module { get; private set; }

      public int Old { get; private set; }

      public int New { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Module = (NwModule) objSelf;
        Old = EventsPlugin.GetEventData("OLD").ParseInt();
        New = EventsPlugin.GetEventData("NEW").ParseInt();
      }
    }

    [NWNXEvent("NWNX_ON_CALENDAR_DAWN")]
    public class OnCalendarDawn : NWNXEvent<OnCalendarDawn>
    {
      public NwModule Module { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Module = (NwModule) objSelf;
    }

    [NWNXEvent("NWNX_ON_CALENDAR_DUSK")]
    public class OnCalendarDusk : NWNXEvent<OnCalendarDusk>
    {
      public NwModule Module { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Module = (NwModule) objSelf;
    }
  }
}
