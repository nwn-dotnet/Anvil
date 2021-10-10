using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiCheck : NuiElement
  {
    public override string Type
    {
      get => "check";
    }

    public NuiCheck(NuiProperty<string> label, NuiProperty<bool> selected)
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
