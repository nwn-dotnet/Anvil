using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A non-editable text field. Supports multiple lines and has a skinned border and a scrollbar if needed.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiText(NuiProperty<string> text) : NuiWidget
  {
    [JsonProperty("value")]
    public NuiProperty<string> Text { get; set; } = text;

    [JsonProperty("border")]
    public bool Border { get; set; } = true;

    [JsonProperty("scrollbars")]
    public NuiScrollbars Scrollbars { get; set; } = NuiScrollbars.Auto;

    public override string Type => "text";
  }
}
