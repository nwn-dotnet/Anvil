using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A circular arc rendered within a draw list.
  /// </summary>
  public sealed class NuiDrawListArc : NuiDrawListItem
  {
    [JsonConstructor]
    public NuiDrawListArc(NuiProperty<Color> color, NuiProperty<bool> fill, NuiProperty<float> lineThickness, NuiProperty<NuiVector> center, NuiProperty<float> radius,
      NuiProperty<float> angleMin, NuiProperty<float> angleMax)
    {
      Color = color;
      Fill = fill;
      LineThickness = lineThickness;
      Center = center;
      Radius = radius;
      AngleMin = angleMin;
      AngleMax = angleMax;
    }

    /// <summary>
    /// Gets or sets the maximum angle of the arc (degrees).
    /// </summary>
    [JsonProperty("amax")]
    public NuiProperty<float> AngleMax { get; set; }

    /// <summary>
    /// Gets or sets the minimum angle of the arc (degrees).
    /// </summary>
    [JsonProperty("amin")]
    public NuiProperty<float> AngleMin { get; set; }

    /// <summary>
    /// Gets or sets the center position.
    /// </summary>
    [JsonProperty("c")]
    public NuiProperty<NuiVector> Center { get; set; }

    /// <summary>
    /// Gets or sets the arc radius.
    /// </summary>
    [JsonProperty("radius")]
    public NuiProperty<float> Radius { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.Arc;
  }
}
