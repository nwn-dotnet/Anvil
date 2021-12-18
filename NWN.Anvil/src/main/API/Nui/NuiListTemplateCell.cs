using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A cell template for use in <see cref="NuiList"/>.
  /// </summary>
  [JsonConverter(typeof(ObjectToArrayConverter<NuiListTemplateCell>))]
  public sealed class NuiListTemplateCell
  {
    [JsonConstructor]
    public NuiListTemplateCell(NuiElement element)
    {
      Element = element;
    }

    /// <summary>
    /// The cell element.
    /// </summary>
    [JsonProperty(Order = 1)]
    public NuiElement Element { get; set; }

    /// <summary>
    /// Gets or sets if this cell can grow if space is available (true), or if it is static (false)
    /// </summary>
    [JsonProperty(Order = 3)]
    public bool VariableSize { get; set; }

    /// <summary>
    /// The width of the cell. 0 = auto, &gt;1 = pixel width.
    /// </summary>
    [JsonProperty(Order = 2)]
    public float Width { get; set; }
  }
}
