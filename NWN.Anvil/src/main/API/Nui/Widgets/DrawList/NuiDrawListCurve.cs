using Newtonsoft.Json;

namespace Anvil.API
{
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

    [JsonProperty("ctrl0")]
    public NuiProperty<NuiVector> Control0 { get; set; }

    [JsonProperty("ctrl1")]
    public NuiProperty<NuiVector> Control1 { get; set; }

    [JsonProperty("a")]
    public NuiProperty<NuiVector> PointA { get; set; }

    [JsonProperty("b")]
    public NuiProperty<NuiVector> PointB { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.Curve;
  }
}
