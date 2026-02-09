using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A tab bar widget presenting selectable toggle elements.
  /// </summary>
  public sealed class NuiToggles(NuiDirection direction, List<string> elements) : NuiWidget
  {
    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "tabbar";

    /// <summary>
    /// Gets or sets the layout direction of the toggles.
    /// </summary>
    [JsonProperty("direction")]
    public NuiDirection Direction { get; set; } = direction;

    /// <summary>
    /// Gets or sets the toggle labels.
    /// </summary>
    [JsonProperty("elements")]
    public List<string> Elements { get; set; } = elements;
  }
}
