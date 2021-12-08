using System;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// Helpers to convert game time units to a TimeSpan.
  /// </summary>
  public static class NwTimeSpan
  {
    /// <summary>
    /// Returns a TimeSpan equal to the specified amount of game hours.
    /// </summary>
    /// <param name="hours">The number of rounds.</param>
    public static TimeSpan FromHours(int hours)
    {
      return TimeSpan.FromSeconds(NWScript.HoursToSeconds(hours));
    }

    /// <summary>
    /// Returns a TimeSpan equal to the specified amount of rounds.
    /// </summary>
    /// <param name="rounds">The number of rounds.</param>
    public static TimeSpan FromRounds(int rounds)
    {
      return TimeSpan.FromSeconds(NWScript.RoundsToSeconds(rounds));
    }

    /// <summary>
    /// Returns a TimeSpan equal to specified amount of turns.
    /// </summary>
    /// <param name="turns">The number of turns.</param>
    public static TimeSpan FromTurns(int turns)
    {
      return TimeSpan.FromSeconds(NWScript.TurnsToSeconds(turns));
    }
  }
}
