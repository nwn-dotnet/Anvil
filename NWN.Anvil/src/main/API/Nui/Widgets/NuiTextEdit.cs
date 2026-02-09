using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// An editable text field. Can be optionally configured as multi-line.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiTextEdit(NuiProperty<string> label, NuiProperty<string> value, ushort maxLength, bool multiLine) : NuiWidget
  {
    /// <summary>
    /// Gets or sets the label text displayed above the field.
    /// </summary>
    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; } = label;

    /// <summary>
    /// Gets or sets the maximum number of characters allowed.
    /// </summary>
    [JsonProperty("max")]
    public ushort MaxLength { get; set; } = maxLength;

    /// <summary>
    /// Gets or sets whether the text field allows multiple lines.
    /// </summary>
    [JsonProperty("multiline")]
    public bool MultiLine { get; set; } = multiLine;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "textedit";

    /// <summary>
    /// Gets or sets the current text value.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<string> Value { get; set; } = value;

    /// <summary>
    /// Gets or sets whether long lines wrap to the next line.
    /// </summary>
    [JsonProperty("wordwrap")]
    public NuiProperty<bool> WordWrap { get; set; } = true;
  }
}
