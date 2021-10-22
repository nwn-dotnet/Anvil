using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiDrawListText : NuiDrawListItem
  {
    public override NuiDrawListItemType Type
    {
      get => NuiDrawListItemType.Text;
    }

    [JsonConstructor]
    public NuiDrawListText(NuiProperty<NuiColor> color, NuiProperty<NuiRect> rect, NuiProperty<string> text) : base(color, null, null)
    {
      Rect = rect;
      Text = text;
    }

    [JsonProperty("rect")]
    public NuiProperty<NuiRect> Rect { get; set; }

    [JsonProperty("text")]
    public NuiProperty<string> Text { get; set; }
  }
}
