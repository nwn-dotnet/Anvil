using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A chart element/data set for use in <see cref="NuiChart"/>.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiChartSlot(NuiChartType chartType, NuiProperty<string> legend, NuiProperty<Color> color, NuiProperty<List<float>> data)
  {
    /// <summary>
    /// Gets or sets the chart series type.
    /// </summary>
    [JsonProperty("type")]
    public NuiChartType ChartType { get; set; } = chartType;

    /// <summary>
    /// Gets or sets the series color.
    /// </summary>
    [JsonProperty("color")]
    public NuiProperty<Color> Color { get; set; } = color;

    /// <summary>
    /// Gets or sets the series data points.
    /// </summary>
    [JsonProperty("data")]
    public NuiProperty<List<float>> Data { get; set; } = data;

    /// <summary>
    /// Gets or sets the series legend label.
    /// </summary>
    [JsonProperty("legend")]
    public NuiProperty<string> Legend { get; set; } = legend;
  }
}
