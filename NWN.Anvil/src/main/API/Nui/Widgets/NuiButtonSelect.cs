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
    /// <summary>
    /// Gets or sets the button label text.
    /// </summary>
    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; } = label;

    /// <summary>
    /// Gets or sets whether this toggle button is selected.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<bool> Selected { get; set; } = selected;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "button_select";
  }
}
