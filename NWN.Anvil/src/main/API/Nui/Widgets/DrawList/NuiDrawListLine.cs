using System.Text.Json.Serialization;

namespace Anvil.API
{
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

    [JsonPropertyName("a")]
    public NuiProperty<NuiVector> PointA { get; set; }

    [JsonPropertyName("b")]
    public NuiProperty<NuiVector> PointB { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.Line;
  }
}
