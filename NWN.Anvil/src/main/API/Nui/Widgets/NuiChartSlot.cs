using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A chart element/data set for use in <see cref="NuiChart"/>.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiChartSlot(NuiChartType chartType, NuiProperty<string> legend, NuiProperty<Color> color, NuiProperty<List<float>> data)
  {
    [JsonPropertyName("type")]
    public NuiChartType ChartType { get; set; } = chartType;

    [JsonPropertyName("color")]
    public NuiProperty<Color> Color { get; set; } = color;

    [JsonPropertyName("data")]
    public NuiProperty<List<float>> Data { get; set; } = data;

    [JsonPropertyName("legend")]
    public NuiProperty<string> Legend { get; set; } = legend;
  }
}
