using Newtonsoft.Json;

namespace Anvil.API
{
  [method: JsonConstructor]
  public sealed class NuiDrawListImage(NuiProperty<string> resRef, NuiProperty<NuiRect> rect) : NuiDrawListItem
  {
    [JsonProperty("image_aspect")]
    public NuiProperty<NuiAspect> Aspect { get; set; } = NuiAspect.Exact;

    [JsonProperty("image_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    [JsonProperty("rect")]
    public NuiProperty<NuiRect> Rect { get; set; } = rect;

    [JsonProperty("image")]
    public NuiProperty<string> ResRef { get; set; } = resRef;

    /// <summary>
    /// Optionally render a subregion of the image.<br/>
    /// This property is a NuiRect (x, y, w, h) to indicate the render region inside the image.
    /// </summary>
    [JsonProperty("image_region", NullValueHandling = NullValueHandling.Ignore)]
    public NuiProperty<NuiRect>? ImageRegion { get; set; }

    public override NuiDrawListItemType Type => NuiDrawListItemType.Image;

    [JsonProperty("image_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;
  }
}
