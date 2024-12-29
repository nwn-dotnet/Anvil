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
    [JsonProperty("type")]
    public NuiChartType ChartType { get; set; } = chartType;

    [JsonProperty("color")]
    public NuiProperty<Color> Color { get; set; } = color;

    [JsonProperty("data")]
    public NuiProperty<List<float>> Data { get; set; } = data;

    [JsonProperty("legend")]
    public NuiProperty<string> Legend { get; set; } = legend;
  }
}
