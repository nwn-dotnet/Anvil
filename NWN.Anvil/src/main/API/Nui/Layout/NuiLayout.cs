using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  [JsonPolymorphic]
  public abstract class NuiLayout : NuiElement
  {
    [JsonPropertyName("children")]
    protected abstract IEnumerable<NuiElement> SerializedChildren { get; }
  }
}
