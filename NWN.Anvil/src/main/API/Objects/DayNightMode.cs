namespace Anvil.API
{
  /// <summary>
  /// Controls how an area handles time-of-day visuals and lighting.
  /// </summary>
  public enum DayNightMode
  {
    /// <summary>
    /// Time progresses normally and the area transitions between day and night.
    /// </summary>
    EnableDayNightCycle,

    /// <summary>
    /// The area is fixed to night visuals; transitions do not occur.
    /// </summary>
    AlwaysNight,

    /// <summary>
    /// The area is fixed to day visuals; transitions do not occur.
    /// </summary>
    AlwaysDay,
  }
}
