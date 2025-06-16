using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A clickable button with an image as the label.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiButtonImage(NuiProperty<string> resRef) : NuiWidget
  {
    [JsonPropertyName("label")]
    public NuiProperty<string> ResRef { get; set; } = resRef;

    [JsonPropertyName("type")]
    public override string Type => "button_image";
  }
}
