using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A layout element that will auto-space all elements inside of it and advise the parent about its desired size.
  /// </summary>
  public sealed class NuiColumn : NuiLayout
  {
    [JsonIgnore]
    public List<NuiElement> Children { get; set; } = [];

    public override string Type => "col";

    protected override IEnumerable<NuiElement> SerializedChildren => Children;
  }
}
