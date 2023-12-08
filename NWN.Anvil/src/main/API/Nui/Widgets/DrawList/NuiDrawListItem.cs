using Newtonsoft.Json;

namespace Anvil.API
{
  public abstract class NuiDrawListItem
  {
    [JsonProperty("color", NullValueHandling = NullValueHandling.Include)]
    public NuiProperty<Color>? Color { get; set; }

    [JsonProperty("enabled")]
    public NuiProperty<bool> Enabled { get; set; } = true;

    [JsonProperty("fill", NullValueHandling = NullValueHandling.Include)]
    public NuiProperty<bool>? Fill { get; set; }

    [JsonProperty("line_thickness", NullValueHandling = NullValueHandling.Include)]
    public NuiProperty<float>? LineThickness { get; set; }

    [JsonProperty("type")]
    public abstract NuiDrawListItemType Type { get; }

    [JsonProperty("order")]
    public NuiDrawListItemOrder Order { get; set; } = NuiDrawListItemOrder.After;

    [JsonProperty("render")]
    public NuiDrawListItemRender Render { get; set; } = NuiDrawListItemRender.Always;
  }
}
