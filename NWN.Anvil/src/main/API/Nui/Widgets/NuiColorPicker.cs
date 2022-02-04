using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A simple color picker, with no borders or spacing.
  /// </summary>
  public sealed class NuiColorPicker : NuiWidget
  {
    [JsonConstructor]
    public NuiColorPicker(NuiProperty<Color> color)
    {
      Color = color;
    }

    [JsonProperty("value")]
    public NuiProperty<Color> Color { get; set; }

    public override string Type => "color_picker";
  }
}
