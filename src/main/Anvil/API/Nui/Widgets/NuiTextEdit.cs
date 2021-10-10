using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiTextEdit : NuiElement
  {
    public override string Type
    {
      get => "textedit";
    }

    public NuiTextEdit(NuiProperty<string> label, NuiProperty<string> value, ushort maxLength, bool multiLine)
    {
      Label = label;
      Value = value;
      MaxLength = maxLength;
      MultiLine = multiLine;
    }

    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; }

    [JsonProperty("value")]
    public NuiProperty<string> Value { get; set; }

    [JsonProperty("max")]
    public ushort MaxLength { get; set; }

    [JsonProperty("multiline")]
    public bool MultiLine { get; set; }
  }
}
