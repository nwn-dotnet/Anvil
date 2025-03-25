using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A clickable button with text as the label.<br/>
  /// Same as <see cref="NuiButton"/>, but this one is a toggle.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiButtonSelect(NuiProperty<string> label, NuiProperty<bool> selected) : NuiWidget
  {
    [JsonPropertyName("label")]
    public NuiProperty<string> Label { get; set; } = label;

    [JsonPropertyName("value")]
    public NuiProperty<bool> Selected { get; set; } = selected;

    public override string Type => "button_select";
  }
}
