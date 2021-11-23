using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiDrawListPolyLine : NuiDrawListItem
  {
    public override NuiDrawListItemType Type
    {
      get => NuiDrawListItemType.PolyLine;
    }

    [JsonConstructor]
    public NuiDrawListPolyLine(NuiProperty<NuiColor> color, NuiProperty<bool> fill, NuiProperty<float> lineThickness, List<float> points) : base(color, fill, lineThickness)
    {
      Points = points;
    }

    [JsonProperty("points")]
    public List<float> Points { get; set; }
  }
}
