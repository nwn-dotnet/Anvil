using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiToggles : NuiWidget
  {
    public override string Type => "tabbar";

    public NuiToggles(NuiDirection direction, List<string> elements)
    {
      Direction = direction;
      Elements = elements;
    }

    [JsonProperty("direction")]
    public NuiDirection Direction { get; set; }

    [JsonProperty("elements")]
    public List<string> Elements { get; set; }
  }
}
