using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A checkbox with a label to the right of it.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiCheck(NuiProperty<string> label, NuiProperty<bool> selected) : NuiWidget
  {
    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; } = label;

    [JsonProperty("value")]
    public NuiProperty<bool> Selected { get; set; } = selected;

    public override string Type => "check";
  }
}
