namespace Anvil.API
{
  /// <summary>
  /// Scrollbar visibility and axis selection for NUI containers.
  /// </summary>
  public enum NuiScrollbars
  {
    /// <summary>
    /// No scrollbars.
    /// </summary>
    None = 0,

    /// <summary>
    /// Horizontal scrollbar only.
    /// </summary>
    X = 1,

    /// <summary>
    /// Vertical scrollbar only.
    /// </summary>
    Y = 2,

    /// <summary>
    /// Both horizontal and vertical scrollbars.
    /// </summary>
    Both = 3,

    /// <summary>
    /// Automatically decide based on content.
    /// </summary>
    Auto = 4,
  }
}
