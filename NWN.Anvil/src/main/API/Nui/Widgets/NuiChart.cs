using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A line/column chart element.
  /// </summary>
  public sealed class NuiChart : NuiWidget
  {
    [JsonPropertyName("value")]
    public List<NuiChartSlot>? ChartSlots { get; set; }

    public override string Type => "chart";
  }
}
