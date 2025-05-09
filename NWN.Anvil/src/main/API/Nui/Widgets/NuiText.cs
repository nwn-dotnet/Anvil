using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A non-editable text field. Supports multiple lines and has a skinned border and a scrollbar if needed.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiText(NuiProperty<string> text) : NuiWidget
  {
    [JsonPropertyName("value")]
    public NuiProperty<string> Text { get; set; } = text;

    [JsonPropertyName("border")]
    public bool Border { get; set; } = true;

    [JsonPropertyName("scrollbars")]
    public NuiScrollbars Scrollbars { get; set; } = NuiScrollbars.Auto;

    [JsonPropertyName("type")]
    public override string Type => "text";
  }
}
