using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiProgress : NuiElement
  {
    public override string Type
    {
      get => "progress";
    }

    public NuiProgress(NuiProperty<float> value)
    {
      Value = value;
    }

    [JsonProperty("value")]
    public NuiProperty<float> Value { get; set; }
  }
}
