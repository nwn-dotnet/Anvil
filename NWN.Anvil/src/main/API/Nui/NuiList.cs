using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A list view of elements.
  /// </summary>
  /// <param name="rowTemplate">The cell template that defines the layout for each row.</param>
  /// <param name="rowCount">A bind controlling the number of rows in the list.</param>
  [method: JsonConstructor]
  public sealed class NuiList(IReadOnlyCollection<NuiListTemplateCell> rowTemplate, NuiProperty<int> rowCount) : NuiElement
  {
    /// <summary>
    /// Gets or sets whether a border should be rendered around this list view.
    /// </summary>
    [JsonProperty("border")]
    public bool Border { get; set; } = true;

    /// <summary>
    /// Gets or sets the number of rows in this list.
    /// </summary>
    [JsonProperty("row_count")]
    public NuiProperty<int> RowCount { get; set; } = rowCount;

    /// <summary>
    /// Gets or sets the row height.
    /// </summary>
    [JsonProperty("row_height")]
    public float RowHeight { get; set; } = NuiStyle.RowHeight;

    /// <summary>
    /// Gets or sets the list of cells composing the row template.<br/>
    /// A max of 16 cells are supported.
    /// </summary>
    [JsonProperty("row_template")]
    public List<NuiListTemplateCell> RowTemplate { get; set; } = [..rowTemplate];

    /// <summary>
    /// Gets or sets whether scroll bars should be rendered for this scroll list.<br/>
    /// <see cref="NuiScrollbars.Auto"/> is not supported.
    /// </summary>
    [JsonProperty("scrollbars")]
    public NuiScrollbars Scrollbars { get; set; } = NuiScrollbars.Y;

    /// <summary>
    /// Gets the NUI element type identifier for serialization.
    /// </summary>
    public override string Type => "list";
  }
}
