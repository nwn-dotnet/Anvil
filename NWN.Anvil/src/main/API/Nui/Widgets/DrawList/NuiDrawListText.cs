using System.Text.Json.Serialization;

namespace Anvil.API
{
  public sealed class NuiDrawListText : NuiDrawListItem
  {
    [JsonConstructor]
    public NuiDrawListText(NuiProperty<Color> color, NuiProperty<NuiRect> rect, NuiProperty<string> text)
    {
      Color = color;
      Rect = rect;
      Text = text;
    }

    [JsonPropertyName("rect")]
    public NuiProperty<NuiRect> Rect { get; set; }

    [JsonPropertyName("text")]
    public NuiProperty<string> Text { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.Text;
  }
}
