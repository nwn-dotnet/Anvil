using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A layout element that will auto-space all elements inside of it and advise the parent about its desired size.
  /// </summary>
  public sealed class NuiRow : NuiLayout
  {
    /// <summary>
    /// Gets the child elements contained within this row.
    /// </summary>
    [JsonIgnore]
    public List<NuiElement> Children { get; set; } = [];

    /// <summary>
    /// Gets the NUI layout type identifier for this element.
    /// </summary>
    public override string Type => "row";

    /// <summary>
    /// Gets the sequence of child elements serialized as the JSON "children" array.
    /// </summary>
    protected override IEnumerable<NuiElement> SerializedChildren => Children;
  }
}
