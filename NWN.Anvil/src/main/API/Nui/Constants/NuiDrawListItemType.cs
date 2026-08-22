namespace Anvil.API
{
  /// <summary>
  /// Draw list item geometry type for NUI rendering.
  /// </summary>
  public enum NuiDrawListItemType
  {
    /// <summary>
    /// A series of connected straight lines.
    /// </summary>
    PolyLine = 0,

    /// <summary>
    /// A curved path between points.
    /// </summary>
    Curve = 1,

    /// <summary>
    /// A circle shape.
    /// </summary>
    Circle = 2,

    /// <summary>
    /// An arc segment of a circle.
    /// </summary>
    Arc = 3,

    /// <summary>
    /// Text rendered at a location.
    /// </summary>
    Text = 4,

    /// <summary>
    /// An image/texture.
    /// </summary>
    Image = 5,

    /// <summary>
    /// A single straight line.
    /// </summary>
    Line = 6,
  }
}
