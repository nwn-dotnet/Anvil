using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A list of options (radio buttons).<br/>
  /// Only one can be selected at a time.
  /// </summary>
  public sealed class NuiOptions : NuiWidget
  {
    /// <summary>
    /// Gets or sets the layout direction of the options.
    /// </summary>
    [JsonProperty("direction")]
    public NuiDirection Direction { get; set; } = NuiDirection.Horizontal;

    /// <summary>
    /// Gets or sets the option labels.
    /// </summary>
    [JsonProperty("elements")]
    public List<string> Options { get; set; } = [];

    /// <summary>
    /// Gets or sets the selected option index.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<int> Selection { get; set; } = -1;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "options";
  }
}
