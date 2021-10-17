using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A chart element/data set for use in <see cref="NuiChart"/>.
  /// </summary>
  public sealed class NuiChartSlot
  {
    public NuiChartSlot(NuiChartType chartType, NuiProperty<string> legend, NuiProperty<NuiColor> color, NuiProperty<List<float>> data)
    {
      ChartType = chartType;
      Legend = legend;
      Color = color;
      Data = data;
    }

    [JsonProperty("type")]
    public NuiChartType ChartType { get; set; }

    [JsonProperty("legend")]
    public NuiProperty<string> Legend { get; set; }

    [JsonProperty("color")]
    public NuiProperty<NuiColor> Color { get; set; }

    [JsonProperty("data")]
    public NuiProperty<List<float>> Data { get; set; }
  }
}
