using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A text label rendered within a draw list.
  /// </summary>
  public sealed class NuiDrawListText : NuiDrawListItem
  {
    [JsonConstructor]
    public NuiDrawListText(NuiProperty<Color> color, NuiProperty<NuiRect> rect, NuiProperty<string> text)
    {
      Color = color;
      Rect = rect;
      Text = text;
    }

    /// <summary>
    /// Gets or sets the bounding rectangle where the text is rendered.
    /// </summary>
    [JsonProperty("rect")]
    public NuiProperty<NuiRect> Rect { get; set; }

    /// <summary>
    /// Gets or sets the text to render.
    /// </summary>
    [JsonProperty("text")]
    public NuiProperty<string> Text { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.Text;
  }
}
