using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// An image, with no border or padding.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiImage(NuiProperty<string> resRef) : NuiWidget
  {
    /// <summary>
    /// Gets or sets the horizontal alignment of the image within its bounds.
    /// </summary>
    [JsonProperty("image_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    /// <summary>
    /// Gets or sets how the image scales within its bounds.
    /// </summary>
    [JsonProperty("image_aspect")]
    public NuiProperty<NuiAspect> ImageAspect { get; set; } = NuiAspect.Exact;

    /// <summary>
    /// Optionally render only subregion of jImage.<br/>
    /// This property is a NuiRect (x, y, w, h) to indicate the render region inside the image.
    /// </summary>
    [JsonProperty("image_region")]
    public NuiProperty<NuiRect>? ImageRegion { get; set; }

    /// <summary>
    /// Gets or sets the image resource reference.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<string> ResRef { get; set; } = resRef;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "image";

    /// <summary>
    /// Gets or sets the vertical alignment of the image within its bounds.
    /// </summary>
    [JsonProperty("image_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;
  }
}
