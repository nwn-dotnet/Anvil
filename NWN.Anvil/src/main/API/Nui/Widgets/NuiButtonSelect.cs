using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A clickable button with text as the label.<br/>
  /// Same as <see cref="NuiButton"/>, but this one is a toggle.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiButtonSelect(NuiProperty<string> label, NuiProperty<bool> selected) : NuiWidget
  {
    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; } = label;

    [JsonProperty("value")]
    public NuiProperty<bool> Selected { get; set; } = selected;

    public override string Type => "button_select";
  }
}
