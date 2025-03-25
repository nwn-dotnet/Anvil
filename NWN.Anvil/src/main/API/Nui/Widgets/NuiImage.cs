using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// An image, with no border or padding.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiImage(NuiProperty<string> resRef) : NuiWidget
  {
    [JsonPropertyName("image_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    [JsonPropertyName("image_aspect")]
    public NuiProperty<NuiAspect> ImageAspect { get; set; } = NuiAspect.Exact;

    /// <summary>
    /// Optionally render only subregion of jImage.<br/>
    /// This property is a NuiRect (x, y, w, h) to indicate the render region inside the image.
    /// </summary>
    [JsonPropertyName("image_region")]
    public NuiProperty<NuiRect>? ImageRegion { get; set; }

    [JsonPropertyName("value")]
    public NuiProperty<string> ResRef { get; set; } = resRef;

    [JsonPropertyName("type")]
    public override string Type => "image";

    [JsonPropertyName("image_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;
  }
}
