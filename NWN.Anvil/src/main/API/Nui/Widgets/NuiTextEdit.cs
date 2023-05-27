using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// An editable text field. Can be optionally configured as multi-line.
  /// </summary>
  public sealed class NuiTextEdit : NuiWidget
  {
    [JsonConstructor]
    public NuiTextEdit(NuiProperty<string> label, NuiProperty<string> value, ushort maxLength, bool multiLine)
    {
      Label = label;
      Value = value;
      MaxLength = maxLength;
      MultiLine = multiLine;
    }

    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; }

    [JsonProperty("max")]
    public ushort MaxLength { get; set; }

    [JsonProperty("multiline")]
    public bool MultiLine { get; set; }

    public override string Type => "textedit";

    [JsonProperty("value")]
    public NuiProperty<string> Value { get; set; }

    [JsonProperty("wordwrap")]
    public NuiProperty<bool> WordWrap { get; set; } = true;
  }
}
