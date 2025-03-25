using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A checkbox with a label to the right of it.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiCheck(NuiProperty<string> label, NuiProperty<bool> selected) : NuiWidget
  {
    [JsonPropertyName("label")]
    public NuiProperty<string> Label { get; set; } = label;

    [JsonPropertyName("value")]
    public NuiProperty<bool> Selected { get; set; } = selected;

    [JsonPropertyName("type")]
    public override string Type => "check";
  }
}
