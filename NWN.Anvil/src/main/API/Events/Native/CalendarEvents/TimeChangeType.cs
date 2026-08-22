namespace Anvil.API.Events
{
  /// <summary>
  /// Identifies which unit of time changed.
  /// </summary>
  public enum TimeChangeType
  {
    /// <summary>
    /// The in-game hour changed.
    /// </summary>
    Hour,

    /// <summary>
    /// The in-game day changed.
    /// </summary>
    Day,

    /// <summary>
    /// The in-game month changed.
    /// </summary>
    Month,

    /// <summary>
    /// The in-game year changed.
    /// </summary>
    Year,

    /// <summary>
    /// The module time state changed (compare against the <see cref="TimeOfDayState"/> constants).
    /// </summary>
    TimeOfDay,
  }
}
