using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// The abstract base for all NUI widgets - the building blocks for creating NUI windows.
  /// </summary>
  public abstract class NuiWidget : NuiElement
  {
    [JsonProperty("draw_list_scissor", NullValueHandling = NullValueHandling.Ignore)]
    public NuiProperty<bool> Scissor { get; set; }

    [JsonProperty("draw_list", NullValueHandling = NullValueHandling.Ignore)]
    public List<NuiDrawListItem> DrawList { get; set; }
  }
}
