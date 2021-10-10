using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiLabel : NuiElement
  {
    public override string Type
    {
      get => "label";
    }

    public NuiLabel(NuiProperty<string> label)
    {
      Label = label;
    }

    [JsonProperty("value")]
    public NuiProperty<string> Label { get; set; }

    [JsonProperty("text_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    [JsonProperty("text_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;
  }
}
