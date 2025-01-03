using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// An editable text field. Can be optionally configured as multi-line.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiTextEdit(NuiProperty<string> label, NuiProperty<string> value, ushort maxLength, bool multiLine) : NuiWidget
  {
    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; } = label;

    [JsonProperty("max")]
    public ushort MaxLength { get; set; } = maxLength;

    [JsonProperty("multiline")]
    public bool MultiLine { get; set; } = multiLine;

    public override string Type => "textedit";

    [JsonProperty("value")]
    public NuiProperty<string> Value { get; set; } = value;

    [JsonProperty("wordwrap")]
    public NuiProperty<bool> WordWrap { get; set; } = true;
  }
}
