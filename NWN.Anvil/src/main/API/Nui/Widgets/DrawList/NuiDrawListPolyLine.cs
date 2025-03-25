using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  public sealed class NuiDrawListPolyLine : NuiDrawListItem
  {
    [JsonConstructor]
    public NuiDrawListPolyLine(NuiProperty<Color> color, NuiProperty<bool> fill, NuiProperty<float> lineThickness, List<float> points)
    {
      Color = color;
      Fill = fill;
      LineThickness = lineThickness;
      Points = points;
    }

    [JsonPropertyName("points")]
    public List<float> Points { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.PolyLine;
  }
}
