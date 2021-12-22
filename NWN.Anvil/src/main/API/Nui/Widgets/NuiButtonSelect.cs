using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A clickable button with text as the label.<br/>
  /// Same as <see cref="NuiButton"/>, but this one is a toggle.
  /// </summary>
  public sealed class NuiButtonSelect : NuiWidget
  {
    [JsonConstructor]
    public NuiButtonSelect(NuiProperty<string> label, NuiProperty<bool> selected)
    {
      Label = label;
      Selected = selected;
    }

    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; }

    [JsonProperty("value")]
    public NuiProperty<bool> Selected { get; set; }

    public override string Type => "button_select";
  }
}
