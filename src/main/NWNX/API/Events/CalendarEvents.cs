using NWN.API;
using NWN.API.Events;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class CalendarEvents
  {
    [NWNXEvent("NWNX_ON_CALENDAR_HOUR")]
    public sealed class OnCalendarHour : IEvent
    {
      public int Old { get; } = EventsPlugin.GetEventData("OLD").ParseInt();

      public int New { get; } = EventsPlugin.GetEventData("NEW").ParseInt();

      NwObject IEvent.Context => null;
    }

    [NWNXEvent("NWNX_ON_CALENDAR_DAY")]
    public sealed class OnCalendarDay : IEvent
    {
      public int Old { get; } = EventsPlugin.GetEventData("OLD").ParseInt();

      public int New { get; } = EventsPlugin.GetEventData("NEW").ParseInt();

      NwObject IEvent.Context => null;
    }

    [NWNXEvent("NWNX_ON_CALENDAR_MONTH")]
    public sealed class OnCalendarMonth : IEvent
    {
      public int Old { get; } = EventsPlugin.GetEventData("OLD").ParseInt();

      public int New { get; } = EventsPlugin.GetEventData("NEW").ParseInt();

      NwObject IEvent.Context => null;
    }

    [NWNXEvent("NWNX_ON_CALENDAR_YEAR")]
    public sealed class OnCalendarYear : IEvent
    {
      public int Old { get; } = EventsPlugin.GetEventData("OLD").ParseInt();

      public int New { get; } = EventsPlugin.GetEventData("NEW").ParseInt();

      NwObject IEvent.Context => null;
    }

    [NWNXEvent("NWNX_ON_CALENDAR_DAWN")]
    public sealed class OnCalendarDawn : IEvent
    {
      NwObject IEvent.Context => null;
    }

    [NWNXEvent("NWNX_ON_CALENDAR_DUSK")]
    public sealed class OnCalendarDusk : IEvent
    {
      NwObject IEvent.Context => null;
    }
  }
}
