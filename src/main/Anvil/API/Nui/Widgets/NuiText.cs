using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiText : NuiElement
  {
    public override string Type
    {
      get => "text";
    }

    public NuiText(NuiProperty<string> text)
    {
      Text = text;
    }

    [JsonProperty("value")]
    public NuiProperty<string> Text { get; set; }
  }
}
