using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiToggles(NuiDirection direction, List<string> elements) : NuiWidget
  {
    public override string Type => "tabbar";

    [JsonProperty("direction")]
    public NuiDirection Direction { get; set; } = direction;

    [JsonProperty("elements")]
    public List<string> Elements { get; set; } = elements;
  }
}
