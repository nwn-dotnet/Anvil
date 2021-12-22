using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A single-line, styleable, non-editable text field.
  /// </summary>
  public sealed class NuiLabel : NuiWidget
  {
    [JsonConstructor]
    public NuiLabel(NuiProperty<string> label)
    {
      Label = label;
    }

    [JsonProperty("text_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    [JsonProperty("value")]
    public NuiProperty<string> Label { get; set; }

    public override string Type => "label";

    [JsonProperty("text_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;
  }
}
