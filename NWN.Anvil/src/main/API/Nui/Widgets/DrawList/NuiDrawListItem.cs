using System.Text.Json.Serialization;

namespace Anvil.API
{
  [JsonPolymorphic]
  public abstract class NuiDrawListItem
  {
    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public NuiProperty<Color>? Color { get; set; }

    [JsonPropertyName("enabled")]
    public NuiProperty<bool> Enabled { get; set; } = true;

    [JsonPropertyName("fill")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public NuiProperty<bool>? Fill { get; set; }

    [JsonPropertyName("line_thickness")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public NuiProperty<float>? LineThickness { get; set; }

    [JsonPropertyName("type")]
    public abstract NuiDrawListItemType Type { get; }

    [JsonPropertyName("order")]
    public NuiDrawListItemOrder Order { get; set; } = NuiDrawListItemOrder.After;

    [JsonPropertyName("render")]
    public NuiDrawListItemRender Render { get; set; } = NuiDrawListItemRender.Always;
  }
}
