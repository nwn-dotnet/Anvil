namespace Anvil.API
{
  /// <summary>
  /// Specifies how content should scale within its container in NUI.
  /// </summary>
  public enum NuiAspect
  {
    /// <summary>
    /// Preserves aspect ratio and fits entirely within the bounds.
    /// </summary>
    Fit = 0,

    /// <summary>
    /// Fills the bounds; may crop to preserve aspect.
    /// </summary>
    Fill = 1,

    /// <summary>
    /// Fits at 100% scale where possible, preserving aspect.
    /// </summary>
    Fit100 = 2,

    /// <summary>
    /// Uses exact size without scaling.
    /// </summary>
    Exact = 3,

    /// <summary>
    /// Uses exact size, scaled by DPI or UI scale.
    /// </summary>
    ExactScaled = 4,

    /// <summary>
    /// Stretches to fill the bounds; aspect ratio may change.
    /// </summary>
    Stretch = 5,
  }
}
