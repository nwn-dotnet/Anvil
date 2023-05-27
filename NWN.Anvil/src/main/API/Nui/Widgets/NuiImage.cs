using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// An image, with no border or padding.
  /// </summary>
  public sealed class NuiImage : NuiWidget
  {
    [JsonConstructor]
    public NuiImage(NuiProperty<string> resRef)
    {
      ResRef = resRef;
    }

    [JsonProperty("image_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    [JsonProperty("image_aspect")]
    public NuiProperty<NuiAspect> ImageAspect { get; set; } = NuiAspect.Exact;

    /// <summary>
    /// Optionally render only subregion of jImage.<br/>
    /// This property is a NuiRect (x, y, w, h) to indicate the render region inside the image.
    /// </summary>
    [JsonProperty("image_region")]
    public NuiProperty<NuiRect>? ImageRegion { get; set; }

    [JsonProperty("value")]
    public NuiProperty<string> ResRef { get; set; }

    public override string Type => "image";

    [JsonProperty("image_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;
  }
}
