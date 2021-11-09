using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A layout element that will auto-space all elements inside of it and advise the parent about its desired size.
  /// </summary>
  public sealed class NuiRow : NuiLayout
  {
    public override string Type { get; } = "row";

    [JsonIgnore]
    public List<NuiElement> Children { get; set; } = new List<NuiElement>();

    protected override IEnumerable<NuiElement> SerializedChildren => Children;
  }
}
