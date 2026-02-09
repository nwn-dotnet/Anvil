using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// Represents an axis-aligned rectangle in NUI coordinate space.
  /// </summary>
  /// <param name="x">The horizontal position of the rectangle's origin.</param>
  /// <param name="y">The vertical position of the rectangle's origin.</param>
  /// <param name="width">The width of the rectangle.</param>
  /// <param name="height">The height of the rectangle.</param>
  [method: JsonConstructor]
  public readonly struct NuiRect(float x, float y, float width, float height)
  {
    /// <summary>
    /// Gets the height of the rectangle.
    /// </summary>
    [JsonProperty("h")]
    public float Height { get; } = height;

    /// <summary>
    /// Gets the width of the rectangle.
    /// </summary>
    [JsonProperty("w")]
    public float Width { get; } = width;

    /// <summary>
    /// Gets the horizontal position of the rectangle's origin.
    /// </summary>
    [JsonProperty("x")]
    public float X { get; } = x;

    /// <summary>
    /// Gets the vertical position of the rectangle's origin.
    /// </summary>
    [JsonProperty("y")]
    public float Y { get; } = y;
  }
}
