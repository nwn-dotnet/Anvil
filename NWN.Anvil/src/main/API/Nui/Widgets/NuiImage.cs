using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// An image, with no border or padding.
  /// </summary>
  public sealed class NuiImage : NuiWidget
  {
    public override string Type
    {
      get => "image";
    }

    [JsonConstructor]
    public NuiImage(NuiProperty<string> resRef)
    {
      ResRef = resRef;
    }

    [JsonProperty("value")]
    public NuiProperty<string> ResRef { get; set; }

    [JsonProperty("image_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    [JsonProperty("image_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;

    [JsonProperty("image_aspect")]
    public NuiProperty<NuiAspect> ImageAspect { get; set; } = NuiAspect.Exact;
  }
}
