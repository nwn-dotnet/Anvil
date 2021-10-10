using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiButtonImage : NuiElement
  {
    public override string Type
    {
      get => "button_image";
    }

    public NuiButtonImage(NuiProperty<string> resRef)
    {
      ResRef = resRef;
    }

    [JsonProperty("label")]
    public NuiProperty<string> ResRef { get; set; }
  }
}
