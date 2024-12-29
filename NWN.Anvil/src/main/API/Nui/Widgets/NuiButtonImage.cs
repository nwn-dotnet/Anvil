using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A clickable button with an image as the label.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiButtonImage(NuiProperty<string> resRef) : NuiWidget
  {
    [JsonProperty("label")]
    public NuiProperty<string> ResRef { get; set; } = resRef;

    public override string Type => "button_image";
  }
}
