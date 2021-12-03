using System;
using NLog;
using NWN.Core;

namespace Anvil.Services
{
  public readonly struct NwDateTime
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public const int DaysInMonth = 28;

    public const long TicksPerDay = TicksPerHour * 24;
    public const long TicksPerHour = TicksPerMinute * 60;
    public const long TicksPerMillisecond = 1;
    public const long TicksPerMinute = TicksPerSecond * 60;
    public const long TicksPerMonth = TicksPerDay * 28;
    public const long TicksPerSecond = TicksPerMillisecond * 1000;
    public const long TicksPerYear = TicksPerMonth * 12;

    public static readonly NwDateTime MaxDate = new NwDateTime(GetTicks(30001)).AddMilliseconds(-1);
    public static readonly NwDateTime MinDate = new NwDateTime(GetTicks());

    /// <summary>
    ///  Gets or sets the current module date and time.
    ///  This date must fall within the years 0 to 32000.
    ///  Time can only be advanced forwards; attempting to set the time backwards
    ///     will result in no change to the calendar, and a warning logged.
    /// </summary>
    public static NwDateTime Now
    {
      get =>
        new NwDateTime(
          NWScript.GetCalendarYear(),
          NWScript.GetCalendarMonth(),
          NWScript.GetCalendarDay(),
          NWScript.GetTimeHour(),
          NWScript.GetTimeMinute(),
          NWScript.GetTimeSecond(),
          NWScript.GetTimeMillisecond());

      set
      {
        if (value < Now)
        {
          Log.Warn("Tried to set date time to a past value! Time will not be modified");
          return;
        }

        NWScript.SetTime(value.Hour, value.Minute, value.Second, value.Millisecond);
        NWScript.SetCalendar(value.Year, value.Month, value.DayInMonth);
      }
    }

    /// <summary>
    /// Gets or sets the current module date.
    /// This date must fall within the years 0 to 32000.
    /// </summary>
    public static NwDateTime Today
    {
      get => Now.Date;
      set => NWScript.SetCalendar(value.Year, value.Month, value.DayInMonth);
    }

    public readonly long Ticks;

    public NwDateTime(int year = 0, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0, int milliSecond = 0)
    {
      Ticks = GetTicks(year, month, day, hour, minute, second, milliSecond);
      ValidateInternal();
    }

    private NwDateTime(long ticks)
    {
      Ticks = ticks;
    }

    public NwDateTime Date
    {
      get => new NwDateTime(Ticks - Ticks % TicksPerDay);
    }

    public int DayInMonth
    {
      get => (int)(Ticks / TicksPerDay % 28) + 1;
    }

    public int DayInTenday
    {
      get
      {
        int dayOfTenday = DayInMonth % 10;
        return dayOfTenday != 0 ? dayOfTenday : 10;
      }
    }

    public int DayInYear
    {
      get => Month * DaysInMonth + DayInMonth;
    }

    public int Hour
    {
      get => (int)(Ticks / TicksPerHour % 24);
    }

    public int Millisecond
    {
      get => (int)(Ticks % 1000);
    }

    public int Minute
    {
      get => (int)(Ticks / TicksPerMinute % 60);
    }

    public int Month
    {
      get => (int)(Ticks / TicksPerMonth % 12) + 1;
    }

    public int Second
    {
      get => (int)(Ticks / TicksPerSecond % 60);
    }

    public int Year
    {
      get => (int)(Ticks / TicksPerYear);
    }

    public static NwDateTime FromTicks(long ticks)
    {
      NwDateTime retVal = new NwDateTime(ticks);
      retVal.ValidateInternal();

      return retVal;
    }

    public static implicit operator long(NwDateTime dateTime)
    {
      return dateTime.Ticks;
    }

    public NwDateTime Add(int value, long scale)
    {
      return new NwDateTime(Ticks + value * scale);
    }

    public NwDateTime AddDays(int days)
    {
      return Add(days, TicksPerDay);
    }

    public NwDateTime AddHours(int hours)
    {
      return Add(hours, TicksPerHour);
    }

    public NwDateTime AddMilliseconds(int milliseconds)
    {
      return Add(milliseconds, 1);
    }

    public NwDateTime AddMinutes(int minutes)
    {
      return Add(minutes, TicksPerMinute);
    }

    public NwDateTime AddMonths(int months)
    {
      return Add(months, TicksPerMonth);
    }

    public NwDateTime AddSeconds(int seconds)
    {
      return Add(seconds, TicksPerSecond);
    }

    public NwDateTime AddYears(int years)
    {
      return Add(years, TicksPerYear);
    }

    public override string ToString()
    {
      return $"{Year}-{Month}-{DayInMonth}T{Hour}:{Minute}:{Second}:{Millisecond}";
    }

    private static long GetTicks(int year = 0, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0, int milliSecond = 0)
    {
      return year * TicksPerYear +
        (month - 1) * TicksPerMonth +
        (day - 1) * TicksPerDay +
        hour * TicksPerHour +
        minute * TicksPerMinute +
        second * TicksPerSecond +
        milliSecond * TicksPerMillisecond;
    }

    private void ValidateInternal()
    {
      if (this > MaxDate || this < MinDate)
      {
        throw new ArgumentOutOfRangeException(nameof(Ticks), $"Value must be between {nameof(MinDate)} and {nameof(MaxDate)}");
      }
    }
  }
}
