using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A non-editable text field. Supports multiple lines and has a skinned border and a scrollbar if needed.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiText(NuiProperty<string> text) : NuiWidget
  {
    /// <summary>
    /// Gets or sets the displayed text.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<string> Text { get; set; } = text;

    /// <summary>
    /// Gets or sets whether a border is rendered around the text container.
    /// </summary>
    [JsonProperty("border")]
    public bool Border { get; set; } = true;

    /// <summary>
    /// Gets or sets the scrollbars for this text container.
    /// </summary>
    [JsonProperty("scrollbars")]
    public NuiScrollbars Scrollbars { get; set; } = NuiScrollbars.Auto;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "text";
  }
}
