using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A list view of elements.
  /// </summary>
  public sealed class NuiList : NuiElement
  {
    public override string Type
    {
      get => "list";
    }

    /// <summary>
    /// Gets or sets the list of cells composing the row template.<br/>
    /// A max of 16 cells are supported.
    /// </summary>
    [JsonProperty("row_template")]
    public List<NuiListTemplateCell> RowTemplate { get; set; }

    /// <summary>
    /// Gets or sets the number of rows in this list.
    /// </summary>
    [JsonProperty("row_count")]
    public NuiProperty<int> RowCount { get; set; }

    /// <summary>
    /// Gets or sets the row height.
    /// </summary>
    [JsonProperty("row_height")]
    public float RowHeight { get; set; } = NuiStyle.RowHeight;

    /// <summary>
    /// Gets or sets whether a border should be rendered around this list view.
    /// </summary>
    [JsonProperty("border")]
    public bool Border { get; set; } = true;

    /// <summary>
    /// Gets or sets whether scroll bars should be rendered for this scroll list.<br/>
    /// <see cref="NuiScrollbars.Auto"/> is not supported.
    /// </summary>
    [JsonProperty("scrollbars")]
    public NuiScrollbars Scrollbars { get; set; } = NuiScrollbars.Y;

    [JsonConstructor]
    public NuiList(IReadOnlyCollection<NuiListTemplateCell> rowTemplate, NuiProperty<int> rowCount)
    {
      RowTemplate = new List<NuiListTemplateCell>(rowTemplate);
      RowCount = rowCount;
    }
  }
}
