using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A list view of elements.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiList(IReadOnlyCollection<NuiListTemplateCell> rowTemplate, NuiProperty<int> rowCount) : NuiElement
  {
    /// <summary>
    /// Gets or sets whether a border should be rendered around this list view.
    /// </summary>
    [JsonPropertyName("border")]
    public bool Border { get; set; } = true;

    /// <summary>
    /// Gets or sets the number of rows in this list.
    /// </summary>
    [JsonPropertyName("row_count")]
    public NuiProperty<int> RowCount { get; set; } = rowCount;

    /// <summary>
    /// Gets or sets the row height.
    /// </summary>
    [JsonPropertyName("row_height")]
    public float RowHeight { get; set; } = NuiStyle.RowHeight;

    /// <summary>
    /// Gets or sets the list of cells composing the row template.<br/>
    /// A max of 16 cells are supported.
    /// </summary>
    [JsonPropertyName("row_template")]
    public List<NuiListTemplateCell> RowTemplate { get; set; } = [..rowTemplate];

    /// <summary>
    /// Gets or sets whether scroll bars should be rendered for this scroll list.<br/>
    /// <see cref="NuiScrollbars.Auto"/> is not supported.
    /// </summary>
    [JsonPropertyName("scrollbars")]
    public NuiScrollbars Scrollbars { get; set; } = NuiScrollbars.Y;

    public override string Type => "list";
  }
}
