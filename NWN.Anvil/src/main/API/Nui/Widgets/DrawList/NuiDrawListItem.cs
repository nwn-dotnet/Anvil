using System.Text.Json.Serialization;

namespace Anvil.API
{
  public abstract class NuiDrawListItem
  {
    [JsonPropertyName("color", NullValueHandling = NullValueHandling.Include)]
    public NuiProperty<Color>? Color { get; set; }

    [JsonPropertyName("enabled")]
    public NuiProperty<bool> Enabled { get; set; } = true;

    [JsonPropertyName("fill", NullValueHandling = NullValueHandling.Include)]
    public NuiProperty<bool>? Fill { get; set; }

    [JsonPropertyName("line_thickness", NullValueHandling = NullValueHandling.Include)]
    public NuiProperty<float>? LineThickness { get; set; }

    [JsonPropertyName("type")]
    public abstract NuiDrawListItemType Type { get; }

    [JsonPropertyName("order")]
    public NuiDrawListItemOrder Order { get; set; } = NuiDrawListItemOrder.After;

    [JsonPropertyName("render")]
    public NuiDrawListItemRender Render { get; set; } = NuiDrawListItemRender.Always;
  }
}
