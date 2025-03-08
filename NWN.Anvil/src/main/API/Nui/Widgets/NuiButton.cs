using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A clickable button with text as the label.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiButton(NuiProperty<string> label) : NuiWidget
  {
    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; } = label;

    public override string Type => "button";
  }
}
