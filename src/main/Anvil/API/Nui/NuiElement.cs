using Newtonsoft.Json;

namespace Anvil.API
{
  public abstract class NuiElement
  {
    [JsonProperty("type")]
    public abstract string Type { get; }

    [JsonProperty("label")]
    public NuiBind<string> Label { get; set; } = null;

    [JsonProperty("value")]
    public NuiBind<string> Value { get; set; } = null;
  }
}
