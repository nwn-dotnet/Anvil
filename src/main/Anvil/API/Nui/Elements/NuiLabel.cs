using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiLabel : NuiElement<string>
  {
    public override string Type { get; } = "label";

    [JsonProperty("text_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = null;

    [JsonProperty("text_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = null;
  }
}
