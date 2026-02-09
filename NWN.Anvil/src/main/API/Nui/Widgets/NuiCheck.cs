using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A checkbox with a label to the right of it.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiCheck(NuiProperty<string> label, NuiProperty<bool> selected) : NuiWidget
  {
    /// <summary>
    /// Gets or sets the checkbox label text.
    /// </summary>
    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; } = label;

    /// <summary>
    /// Gets or sets whether the checkbox is selected.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<bool> Selected { get; set; } = selected;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "check";
  }
}
