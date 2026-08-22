using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// Base class for NUI layout elements that contain child elements.
  /// </summary>
  public abstract class NuiLayout : NuiElement
  {
    /// <summary>
    /// Gets the sequence of child elements to be serialized under the JSON property "children".
    /// </summary>
    [JsonProperty("children")]
    protected abstract IEnumerable<NuiElement> SerializedChildren { get; }
  }
}
