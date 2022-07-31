using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A non-editable text field. Supports multiple lines and has a skinned border and a scrollbar if needed.
  /// </summary>
  public sealed class NuiText : NuiWidget
  {
    [JsonConstructor]
    public NuiText(NuiProperty<string> text)
    {
      Text = text;
    }

    [JsonProperty("value")]
    public NuiProperty<string> Text { get; set; }

    [JsonProperty("border")]
    public bool Border { get; set; } = true;

    [JsonProperty("scrollbars")]
    public NuiScrollbars Scrollbars { get; set; } = NuiScrollbars.Auto;

    public override string Type => "text";
  }
}
