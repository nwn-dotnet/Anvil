using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A clickable button with text as the label.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiButton(NuiProperty<string> label) : NuiWidget
  {
    [JsonPropertyName("label")]
    public NuiProperty<string> Label { get; set; } = label;

    [JsonPropertyName("type")]
    public override string Type => "button";
  }
}
