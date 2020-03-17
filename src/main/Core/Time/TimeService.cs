using System;
using NLog;
using NWN;

namespace NWM.Core
{
  [Service]
  public class TimeService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public NwnDateTime Now
    {
      get
      {
        return new NwnDateTime(
          NWScript.GetCalendarYear(),
          NWScript.GetCalendarMonth(),
          NWScript.GetCalendarDay(),
          NWScript.GetTimeHour(),
          NWScript.GetTimeMinute(),
          NWScript.GetTimeSecond(),
          NWScript.GetTimeMillisecond());
      }
      set
      {
        if (value < Now)
        {
          Log.Warn("Tried to set date time to a past value! Time will not be modified.");
          return;
        }

        NWScript.SetTime(value.Hour, value.Minute, value.Second, value.Millisecond);
        NWScript.SetCalendar(value.Year, value.Month, value.Day);
      }
    }
  }
}