using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A dropdown menu/combobox.
  /// </summary>
  public sealed class NuiCombo : NuiWidget
  {
    [JsonProperty("elements")]
    public NuiProperty<List<NuiComboEntry>> Entries { get; set; } = new List<NuiComboEntry>();

    [JsonProperty("value")]
    public NuiProperty<int> Selected { get; set; } = 0;

    public override string Type
    {
      get => "combo";
    }
  }
}
