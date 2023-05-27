using Newtonsoft.Json;

namespace Anvil.API
{
  public abstract class NuiDrawListItem
  {
    protected NuiDrawListItem(NuiProperty<Color>? color, NuiProperty<bool>? fill, NuiProperty<float>? lineThickness)
    {
      Color = color;
      Fill = fill;
      LineThickness = lineThickness;
    }

    [JsonProperty("color")]
    public NuiProperty<Color>? Color { get; set; }

    [JsonProperty("enabled")]
    public NuiProperty<bool> Enabled { get; set; } = true;

    [JsonProperty("fill")]
    public NuiProperty<bool>? Fill { get; set; }

    [JsonProperty("line_thickness")]
    public NuiProperty<float>? LineThickness { get; set; }

    [JsonProperty("type")]
    public abstract NuiDrawListItemType Type { get; }

    [JsonProperty("order")]
    public NuiDrawListItemOrder Order { get; set; } = NuiDrawListItemOrder.After;

    [JsonProperty("render")]
    public NuiDrawListItemRender Render { get; set; } = NuiDrawListItemRender.Always;
  }
}
