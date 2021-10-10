using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A line/column chart element.
  /// </summary>
  public sealed class NuiChart : NuiElement
  {
    public override string Type
    {
      get => "chart";
    }

    [JsonProperty("value")]
    public List<NuiChartSlot> ChartSlots { get; set; }
  }
}
