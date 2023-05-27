using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiDrawListLine : NuiDrawListItem
  {
    [JsonConstructor]
    public NuiDrawListLine(NuiProperty<Color> color, NuiProperty<bool> fill, NuiProperty<float> lineThickness, NuiProperty<NuiVector> pointA, NuiProperty<NuiVector> pointB) : base(color, fill, lineThickness)
    {
      PointA = pointA;
      PointB = pointB;
    }

    [JsonProperty("a")]
    public NuiProperty<NuiVector> PointA { get; set; }

    [JsonProperty("b")]
    public NuiProperty<NuiVector> PointB { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.Line;
  }
}
