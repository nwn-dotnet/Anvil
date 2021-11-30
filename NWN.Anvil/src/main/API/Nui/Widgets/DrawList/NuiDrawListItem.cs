using Newtonsoft.Json;

namespace Anvil.API
{
  public abstract class NuiDrawListItem
  {
    protected NuiDrawListItem(NuiProperty<NuiColor> color, NuiProperty<bool> fill, NuiProperty<float> lineThickness)
    {
      Color = color;
      Fill = fill;
      LineThickness = lineThickness;
    }

    [JsonProperty("color")]
    public NuiProperty<NuiColor> Color { get; set; }

    [JsonProperty("enabled")]
    public NuiProperty<bool> Enabled { get; set; } = true;

    [JsonProperty("fill")]
    public NuiProperty<bool> Fill { get; set; }

    [JsonProperty("line_thickness")]
    public NuiProperty<float> LineThickness { get; set; }

    [JsonProperty("type")]
    public abstract NuiDrawListItemType Type { get; }
  }
}
