using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiButtonSelect : NuiElement
  {
    public override string Type
    {
      get => "button_select";
    }

    public NuiButtonSelect(NuiProperty<string> label, NuiProperty<bool> selected)
    {
      Label = label;
      Selected = selected;
    }

    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; }

    [JsonProperty("value")]
    public NuiProperty<bool> Selected { get; set; }
  }
}
