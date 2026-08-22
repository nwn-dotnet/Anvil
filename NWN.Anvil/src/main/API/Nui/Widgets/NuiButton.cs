using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A clickable button with text as the label.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiButton(NuiProperty<string> label) : NuiWidget
  {
    /// <summary>
    /// Gets or sets the button label text.
    /// </summary>
    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; } = label;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "button";
  }
}
