using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A dropdown menu/combobox.
  /// </summary>
  public sealed class NuiCombo : NuiWidget
  {
    [JsonPropertyName("elements")]
    public NuiProperty<List<NuiComboEntry>> Entries { get; set; } = new List<NuiComboEntry>();

    [JsonPropertyName("value")]
    public NuiProperty<int> Selected { get; set; } = 0;

    [JsonPropertyName("type")]
    public override string Type => "combo";
  }
}
