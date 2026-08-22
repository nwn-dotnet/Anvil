using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A single-line, styleable, non-editable text field.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiLabel(NuiProperty<string> label) : NuiWidget
  {
    /// <summary>
    /// Gets or sets the horizontal alignment of the text.
    /// </summary>
    [JsonProperty("text_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    /// <summary>
    /// Gets or sets the label text.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<string> Label { get; set; } = label;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "label";

    /// <summary>
    /// Gets or sets the vertical alignment of the text.
    /// </summary>
    [JsonProperty("text_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;
  }
}
