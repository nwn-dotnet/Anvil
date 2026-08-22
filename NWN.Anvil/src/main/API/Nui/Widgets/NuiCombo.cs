using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A dropdown menu/combobox.
  /// </summary>
  public sealed class NuiCombo : NuiWidget
  {
    /// <summary>
    /// Gets or sets the list of combo entries to display.
    /// </summary>
    [JsonProperty("elements")]
    public NuiProperty<List<NuiComboEntry>> Entries { get; set; } = new List<NuiComboEntry>();

    /// <summary>
    /// Gets or sets the selected entry index.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<int> Selected { get; set; } = 0;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "combo";
  }
}
