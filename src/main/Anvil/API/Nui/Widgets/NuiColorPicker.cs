using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A simple color picker, with no borders or spacing.
  /// </summary>
  public sealed class NuiColorPicker : NuiElement
  {
    public override string Type
    {
      get => "color_picker";
    }

    public NuiColorPicker(NuiProperty<NuiColor> color)
    {
      Color = color;
    }

    [JsonProperty("value")]
    public NuiProperty<NuiColor> Color { get; set; }
  }
}
