using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiDrawListImage : NuiDrawListItem
  {
    [JsonConstructor]
    public NuiDrawListImage(NuiProperty<string> resRef, NuiProperty<NuiRect> rect) : base(null, null, null)
    {
      ResRef = resRef;
      Rect = rect;
    }

    [JsonProperty("image_aspect")]
    public NuiProperty<NuiAspect> Aspect { get; set; } = NuiAspect.Exact;

    [JsonProperty("image_halign")]
    public NuiProperty<NuiHAlign> HorizontalAlign { get; set; } = NuiHAlign.Left;

    [JsonProperty("rect")]
    public NuiProperty<NuiRect> Rect { get; set; }

    [JsonProperty("image")]
    public NuiProperty<string> ResRef { get; set; }

    public override NuiDrawListItemType Type
    {
      get => NuiDrawListItemType.Image;
    }

    [JsonProperty("image_valign")]
    public NuiProperty<NuiVAlign> VerticalAlign { get; set; } = NuiVAlign.Top;
  }
}
