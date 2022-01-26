using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiDrawListPolyLine : NuiDrawListItem
  {
    [JsonConstructor]
    public NuiDrawListPolyLine(NuiProperty<Color> color, NuiProperty<bool> fill, NuiProperty<float> lineThickness, List<float> points) : base(color, fill, lineThickness)
    {
      Points = points;
    }

    [JsonProperty("points")]
    public List<float> Points { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.PolyLine;
  }
}
