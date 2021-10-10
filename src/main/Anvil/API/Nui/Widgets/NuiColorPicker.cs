using Newtonsoft.Json;

namespace Anvil.API
{
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
