using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A circle rendered within a draw list.
  /// </summary>
  public sealed class NuiDrawListCircle : NuiDrawListItem
  {
    [JsonConstructor]
    public NuiDrawListCircle(NuiProperty<Color> color, NuiProperty<bool> fill, NuiProperty<float> lineThickness, NuiProperty<NuiRect> rect)
    {
      Color = color;
      Fill = fill;
      LineThickness = lineThickness;
      Rect = rect;
    }

    /// <summary>
    /// Gets or sets the bounding rectangle of the circle.
    /// </summary>
    [JsonProperty("rect")]
    public NuiProperty<NuiRect> Rect { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.Circle;
  }
}
