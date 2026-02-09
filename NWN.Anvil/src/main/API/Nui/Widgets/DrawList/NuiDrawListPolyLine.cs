using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A sequence of connected straight line segments rendered within a draw list.
  /// </summary>
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

    /// <summary>
    /// Gets or sets the flattened list of points (x,y pairs).
    /// </summary>
    [JsonProperty("points")]
    public List<float> Points { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.PolyLine;
  }
}
