using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A simple color picker, with no borders or spacing.
  /// </summary>
  public sealed class NuiColorPicker : NuiWidget
  {
    [JsonConstructor]
    public NuiColorPicker(NuiProperty<NuiColor> color)
    {
      Color = color;
    }

    [JsonProperty("value")]
    public NuiProperty<NuiColor> Color { get; set; }

    public override string Type => "color_picker";
  }
}
