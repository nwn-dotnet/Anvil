using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A line/column chart element.
  /// </summary>
  public sealed class NuiChart : NuiWidget
  {
    /// <summary>
    /// Gets or sets the chart slots (series) to render.
    /// </summary>
    [JsonProperty("value")]
    public List<NuiChartSlot>? ChartSlots { get; set; }

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "chart";
  }
}
