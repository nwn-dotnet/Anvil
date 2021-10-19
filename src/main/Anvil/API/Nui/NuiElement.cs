using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A NUI widget/element.
  /// </summary>
  public abstract class NuiElement
  {
    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public abstract string Type { get; }

    /// <summary>
    /// A unique identifier for this element.
    /// </summary>
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    /// <summary>
    /// The width of this element, in pixels.
    /// </summary>
    [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
    public float? Width { get; set; }

    /// <summary>
    /// The height of this element, in pixels.
    /// </summary>
    [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
    public float? Height { get; set; }

    /// <summary>
    /// The aspect ratio (x/y) for this element.
    /// </summary>
    [JsonProperty("aspect", NullValueHandling = NullValueHandling.Ignore)]
    public float? Aspect { get; set; }

    /// <summary>
    /// The margin on the widget. The margin is the spacing outside of the widget.
    /// </summary>
    [JsonProperty("margin", NullValueHandling = NullValueHandling.Ignore)]
    public float? Margin { get; set; }

    /// <summary>
    /// The padding on the widget. The padding is the spacing inside of the widget.
    /// </summary>
    [JsonProperty("padding", NullValueHandling = NullValueHandling.Ignore)]
    public float? Padding { get; set; }

    /// <summary>
    /// Toggles if this element is active/interactable, or disabled/greyed out.
    /// </summary>
    [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
    public NuiProperty<bool> Enabled { get; set; }

    /// <summary>
    /// Toggles if this element should/should not be rendered. Invisible elements still take up layout space, and cannot be clicked through.
    /// </summary>
    [JsonProperty("visible", NullValueHandling = NullValueHandling.Ignore)]
    public NuiProperty<bool> Visible { get; set; }

    /// <summary>
    /// A tooltip to show when hovering over this element.
    /// </summary>
    [JsonProperty("tooltip", NullValueHandling = NullValueHandling.Ignore)]
    public NuiProperty<string> Tooltip { get; set; }

    /// <summary>
    /// Style the foreground color of this widget.<br/>
    /// This is dependent on the widget in question and only supports solid/full colors right now (no texture skinning).<br/>
    /// For example, labels would style their text color; progress bars would style the bar.
    /// </summary>
    [JsonProperty("foreground_color", NullValueHandling = NullValueHandling.Ignore)]
    public NuiProperty<NuiColor> ForegroundColor { get; set; }

    [JsonProperty("draw_list_scissor", NullValueHandling = NullValueHandling.Ignore)]
    public NuiProperty<bool> Scissor { get; set; }

    [JsonProperty("draw_list", NullValueHandling = NullValueHandling.Ignore)]
    public List<NuiDrawListItem> DrawList { get; set; }
  }
}
