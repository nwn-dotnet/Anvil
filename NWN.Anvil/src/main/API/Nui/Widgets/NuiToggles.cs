using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  public sealed class NuiToggles(NuiDirection direction, List<string> elements) : NuiWidget
  {
    [JsonPropertyName("type")]
    public override string Type => "tabbar";

    [JsonPropertyName("direction")]
    public NuiDirection Direction { get; set; } = direction;

    [JsonPropertyName("elements")]
    public List<string> Elements { get; set; } = elements;
  }
}
