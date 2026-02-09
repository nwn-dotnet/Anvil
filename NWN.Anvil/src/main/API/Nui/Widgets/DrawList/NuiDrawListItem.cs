using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// The abstract base for draw list items rendered within a NUI draw list.
  /// </summary>
  public abstract class NuiDrawListItem
  {
    /// <summary>
    /// Gets or sets the render color for this item.
    /// </summary>
    [JsonProperty("color", NullValueHandling = NullValueHandling.Include)]
    public NuiProperty<Color>? Color { get; set; }

    /// <summary>
    /// Gets or sets whether this item is enabled for rendering.
    /// </summary>
    [JsonProperty("enabled")]
    public NuiProperty<bool> Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets whether this item is filled (for shapes that support a fill).
    /// </summary>
    [JsonProperty("fill", NullValueHandling = NullValueHandling.Include)]
    public NuiProperty<bool>? Fill { get; set; }

    /// <summary>
    /// Gets or sets the thickness of lines used for rendering.
    /// </summary>
    [JsonProperty("line_thickness", NullValueHandling = NullValueHandling.Include)]
    public NuiProperty<float>? LineThickness { get; set; }

    /// <summary>
    /// Gets the draw list item type.
    /// </summary>
    [JsonProperty("type")]
    public abstract NuiDrawListItemType Type { get; }

    /// <summary>
    /// Gets or sets the relative render order of this item.
    /// </summary>
    [JsonProperty("order")]
    public NuiDrawListItemOrder Order { get; set; } = NuiDrawListItemOrder.After;

    /// <summary>
    /// Gets or sets the render condition for this item.
    /// </summary>
    [JsonProperty("render")]
    public NuiDrawListItemRender Render { get; set; } = NuiDrawListItemRender.Always;
  }
}
