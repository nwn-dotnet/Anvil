namespace Anvil.API
{
  /// <summary>
  /// Relative render order for draw list items.
  /// </summary>
  public enum NuiDrawListItemOrder
  {
    /// <summary>
    /// Render before the default order.
    /// </summary>
    Before = -1,

    /// <summary>
    /// Render in the default order.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Render after the default order.
    /// </summary>
    After = 1,
  }
}
