using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A clickable button with an image as the label.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiButtonImage(NuiProperty<string> resRef) : NuiWidget
  {
    /// <summary>
    /// Gets or sets the image resource reference used as the button label.
    /// </summary>
    [JsonProperty("label")]
    public NuiProperty<string> ResRef { get; set; } = resRef;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "button_image";
  }
}
