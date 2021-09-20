using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiLabel : NuiElement
  {
    public override string Type { get; } = "label";

    [JsonProperty("text_halign")]
    public NuiHAlign? HorizontalAlign { get; set; } = null;

    [JsonProperty("text_valign")]
    public NuiVAlign? VerticalAlign { get; set; } = null;

    public NuiLabel()
    {
      Value = new NuiBind<string>();
    }
  }
}
