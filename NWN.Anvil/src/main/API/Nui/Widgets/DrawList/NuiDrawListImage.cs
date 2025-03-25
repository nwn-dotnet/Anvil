using System.Text.Json.Serialization;

namespace Anvil.API
{
  [method: JsonConstructor]
  public sealed class NuiDrawListImage(NuiProperty<string> resRef, NuiProperty<NuiRect> rect) : NuiDrawListItem
  {
    [JsonPropertyName("image_aspect")]
    public NuiProperty<NuiAspect> Aspect { get; set; } = NuiAspect.Exact;

    [JsonPropertyName("image_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    [JsonPropertyName("rect")]
    public NuiProperty<NuiRect> Rect { get; set; } = rect;

    [JsonPropertyName("image")]
    public NuiProperty<string> ResRef { get; set; } = resRef;

    /// <summary>
    /// Optionally render a subregion of the image.<br/>
    /// This property is a NuiRect (x, y, w, h) to indicate the render region inside the image.
    /// </summary>
    [JsonPropertyName("image_region")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NuiProperty<NuiRect>? ImageRegion { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.Image;

    [JsonPropertyName("image_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;
  }
}
