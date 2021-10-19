using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiDrawListImage : NuiDrawListItem
  {
    public override NuiDrawListItemType Type
    {
      get => NuiDrawListItemType.Image;
    }

    [JsonProperty("image")]
    public NuiProperty<string> ResRef { get; set; }

    [JsonProperty("rect")]
    public NuiProperty<NuiRect> Rect { get; set; }

    [JsonProperty("image_aspect")]
    public NuiProperty<NuiAspect> Aspect { get; set; } = NuiAspect.Exact;

    [JsonProperty("image_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    [JsonProperty("image_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;

    [JsonConstructor]
    public NuiDrawListImage(NuiProperty<string> resRef, NuiProperty<NuiRect> rect) : base(null, null, null)
    {
      ResRef = resRef;
      Rect = rect;
    }
  }
}
