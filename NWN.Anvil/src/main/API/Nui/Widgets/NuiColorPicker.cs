using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A simple color picker, with no borders or spacing.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiColorPicker(NuiProperty<Color> color) : NuiWidget
  {
    /// <summary>
    /// Gets or sets the current color value.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<Color> Color { get; set; } = color;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "color_picker";
  }
}
