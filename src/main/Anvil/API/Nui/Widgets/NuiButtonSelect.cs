using Newtonsoft.Json;

namespace Anvil.API
{
  public class NuiButtonSelect : NuiElement
  {
    public override string Type
    {
      get => "button_select";
    }

    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; }

    [JsonProperty("value")]
    public NuiProperty<bool> Value { get; set; }
  }
}
