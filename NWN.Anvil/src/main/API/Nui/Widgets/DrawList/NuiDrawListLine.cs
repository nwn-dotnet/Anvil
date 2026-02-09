using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A straight line rendered within a draw list.
  /// </summary>
  public sealed class NuiDrawListLine : NuiDrawListItem
  {
    [JsonConstructor]
    public NuiDrawListLine(NuiProperty<Color> color, NuiProperty<bool> fill, NuiProperty<float> lineThickness, NuiProperty<NuiVector> pointA, NuiProperty<NuiVector> pointB)
    {
      Color = color;
      Fill = fill;
      LineThickness = lineThickness;
      PointA = pointA;
      PointB = pointB;
    }

    /// <summary>
    /// Gets or sets the start point.
    /// </summary>
    [JsonProperty("a")]
    public NuiProperty<NuiVector> PointA { get; set; }

    /// <summary>
    /// Gets or sets the end point.
    /// </summary>
    [JsonProperty("b")]
    public NuiProperty<NuiVector> PointB { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.Line;
  }
}
