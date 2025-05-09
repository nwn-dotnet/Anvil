using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// An editable text field. Can be optionally configured as multi-line.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiTextEdit(NuiProperty<string> label, NuiProperty<string> value, ushort maxLength, bool multiLine) : NuiWidget
  {
    [JsonPropertyName("label")]
    public NuiProperty<string> Label { get; set; } = label;

    [JsonPropertyName("max")]
    public ushort MaxLength { get; set; } = maxLength;

    [JsonPropertyName("multiline")]
    public bool MultiLine { get; set; } = multiLine;

    [JsonPropertyName("type")]
    public override string Type => "textedit";

    [JsonPropertyName("value")]
    public NuiProperty<string> Value { get; set; } = value;

    [JsonPropertyName("wordwrap")]
    public NuiProperty<bool> WordWrap { get; set; } = true;
  }
}
