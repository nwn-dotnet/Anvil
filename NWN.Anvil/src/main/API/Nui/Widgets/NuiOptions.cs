using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A list of options (radio buttons).<br/>
  /// Only one can be selected at a time.
  /// </summary>
  public sealed class NuiOptions : NuiWidget
  {
    [JsonPropertyName("direction")]
    public NuiDirection Direction { get; set; } = NuiDirection.Horizontal;

    [JsonPropertyName("elements")]
    public List<string> Options { get; set; } = [];

    [JsonPropertyName("value")]
    public NuiProperty<int> Selection { get; set; } = -1;

    [JsonPropertyName("type")]
    public override string Type => "options";
  }
}
