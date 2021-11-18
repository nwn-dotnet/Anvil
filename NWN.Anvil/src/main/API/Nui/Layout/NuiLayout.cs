using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  public abstract class NuiLayout : NuiElement
  {
    [JsonProperty("children")]
    protected abstract IEnumerable<NuiElement> SerializedChildren { get; }
  }
}
