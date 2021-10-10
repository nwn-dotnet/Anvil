using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A group, usually with a border and some padding, holding a single element. Can scroll.<br/>
  /// Will not advise parent of size, so you need to let it fill a span (col/row) as if it was a element.
  /// </summary>
  public sealed class NuiGroup : NuiLayout
  {
    public override string Type { get; } = "group";

    [JsonProperty("border")]
    public bool Border { get; set; } = true;

    [JsonProperty("scrollbars")]
    public NuiScrollbars Scrollbars { get; set; } = NuiScrollbars.Auto;
  }
}
