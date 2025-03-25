using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A simple color picker, with no borders or spacing.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiColorPicker(NuiProperty<Color> color) : NuiWidget
  {
    [JsonPropertyName("value")]
    public NuiProperty<Color> Color { get; set; } = color;

    [JsonPropertyName("type")]
    public override string Type => "color_picker";
  }
}
