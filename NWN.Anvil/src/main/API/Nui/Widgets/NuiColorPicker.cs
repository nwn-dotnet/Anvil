using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A simple color picker, with no borders or spacing.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiColorPicker(NuiProperty<Color> color) : NuiWidget
  {
    [JsonProperty("value")]
    public NuiProperty<Color> Color { get; set; } = color;

    public override string Type => "color_picker";
  }
}
