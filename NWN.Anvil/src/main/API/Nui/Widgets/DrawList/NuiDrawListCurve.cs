using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A Bezier curve rendered within a draw list.
  /// </summary>
  public sealed class NuiDrawListCurve : NuiDrawListItem
  {
    [JsonConstructor]
    public NuiDrawListCurve(NuiProperty<Color> color, NuiProperty<float> lineThickness, NuiProperty<NuiVector> pointA, NuiProperty<NuiVector> pointB, NuiProperty<NuiVector> control0, NuiProperty<NuiVector> control1)
    {
      Color = color;
      Fill = false;
      LineThickness = lineThickness;
      PointA = pointA;
      PointB = pointB;
      Control0 = control0;
      Control1 = control1;
    }

    /// <summary>
    /// Gets or sets the first control point.
    /// </summary>
    [JsonProperty("ctrl0")]
    public NuiProperty<NuiVector> Control0 { get; set; }

    /// <summary>
    /// Gets or sets the second control point.
    /// </summary>
    [JsonProperty("ctrl1")]
    public NuiProperty<NuiVector> Control1 { get; set; }

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

    public override NuiDrawListItemType Type => NuiDrawListItemType.Curve;
  }
}
