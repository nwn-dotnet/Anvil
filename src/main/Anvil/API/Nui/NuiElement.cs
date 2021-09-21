using Newtonsoft.Json;

namespace Anvil.API
{
  public abstract class NuiElement
  {
    [JsonProperty("type")]
    public abstract string Type { get; }

    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; } = null;
  }

  public abstract class NuiElement<T> : NuiElement
  {
    [JsonProperty("value")]
    public NuiProperty<T> Value { get; set; } = null;
  }
}
