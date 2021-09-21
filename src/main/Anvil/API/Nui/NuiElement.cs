using Newtonsoft.Json;

namespace Anvil.API
{
  public abstract class NuiElement
  {
    [JsonProperty("type")]
    public abstract string Type { get; }
  }
}
