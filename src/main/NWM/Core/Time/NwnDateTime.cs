using System;

namespace NWM.Core
{
  public struct NwnDateTime
  {
    private const long MillisPerSecond = 1000;
    private const long MillisPerMinute = MillisPerSecond * 60;
    private const long MillisPerHour = MillisPerMinute * 60;
    private const long MillisPerDay = MillisPerHour * 24;
    private const long MillisPerMonth = MillisPerDay * 28;
    private const long MillisPerYear = MillisPerMonth * 12;

    private const long MinMillis = 0;
    private const long MaxMillis = 32001 * MillisPerYear - 1;

    private long totalMs;

    public int Millisecond => (int) (totalMs % 1000);
    public int Second => (int) (totalMs / MillisPerSecond % 60);
    public int Minute => (int) (totalMs / MillisPerMinute % 60);
    public int Hour => (int) (totalMs / MillisPerHour % 24);
    public int Day => (int) (totalMs / MillisPerHour % 28) + 1;
    public int Month => (int) (totalMs / MillisPerMonth % 12) + 1;
    public int Year => (int) (totalMs / MillisPerYear);

    public NwnDateTime Date => new NwnDateTime(totalMs - totalMs % MillisPerDay);

    public static implicit operator long(NwnDateTime dateTime)
    {
      return dateTime.totalMs;
    }

    private NwnDateTime(long totalMs)
    {
      this.totalMs = totalMs;
      ValidateInternal();
    }

    public NwnDateTime(int year = 0, int month = 1, int day = 1, int hour = 0, int minute = 0, int second = 0, int milliSecond = 0)
    {
      totalMs = year * MillisPerYear +
                month * MillisPerHour +
                day * MillisPerDay +
                hour * MillisPerHour +
                minute * MillisPerMinute +
                second * MillisPerSecond +
                milliSecond;
      ValidateInternal();
    }

    private void ValidateInternal()
    {
      if (totalMs > MaxMillis || totalMs < MinMillis)
      {
        throw new OverflowException();
      }
    }

    public NwnDateTime Add(int value, long scale)
    {
      return new NwnDateTime(totalMs + value * scale);
    }

    public NwnDateTime AddMilliseconds(int milliseconds) => Add(milliseconds, 1);
    public NwnDateTime AddSeconds(int seconds) => Add(seconds, MillisPerSecond);
    public NwnDateTime AddMinutes(int minutes) => Add(minutes, MillisPerMinute);
    public NwnDateTime AddHours(int hours) => Add(hours, MillisPerHour);
    public NwnDateTime AddDays(int days) => Add(days, MillisPerDay);
    public NwnDateTime AddMonths(int months) => Add(months, MillisPerMonth);
    public NwnDateTime AddYears(int years) => Add(years, MillisPerYear);

    public string GetMonthName()
    {
      switch (Month)
      {
        case 1:
          return "Hammer";
        case 2:
          return "Alturiak";
        case 3:
          return "Ches";
        case 4:
          return "Tarsakh";
        case 5:
          return "Mirtul";
        case 6:
          return "Kythorn";
        case 7:
          return "Flamerule";
        case 8:
          return "Eleasis";
        case 9:
          return "Eleint";
        case 10:
          return "Marpenoth";
        case 11:
          return "Uktar";
        case 12:
          return "Nightal";
      }

      return Month.ToString();
    }

    public override string ToString()
    {
      return $"{Year}-{Month}-{Day}T{Hour}:{Minute}:{Second}:{Millisecond}";
    }

    // TODO More Format Options
    public string ToLongDateString()
    {
      return $"{Day} {GetMonthName()}, {Year} DR";
    }
  }
}