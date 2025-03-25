using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A single-line, styleable, non-editable text field.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiLabel(NuiProperty<string> label) : NuiWidget
  {
    [JsonPropertyName("text_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    [JsonPropertyName("value")]
    public NuiProperty<string> Label { get; set; } = label;

    public override string Type => "label";

    [JsonPropertyName("text_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;
  }
}
