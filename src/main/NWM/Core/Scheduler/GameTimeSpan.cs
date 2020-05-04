using System;
using NWN;

namespace NWM.Core
{
  public static class GameTimeSpan
  {
    public static TimeSpan FromRounds(int rounds)
    {
      return TimeSpan.FromSeconds(NWScript.RoundsToSeconds(rounds));
    }

    public static TimeSpan FromHours(int hours)
    {
      return TimeSpan.FromSeconds(NWScript.HoursToSeconds(hours));
    }

    public static TimeSpan FromTurns(int turns)
    {
      return TimeSpan.FromSeconds(NWScript.TurnsToSeconds(turns));
    }
  }
}