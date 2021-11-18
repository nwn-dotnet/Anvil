using System.Collections.Generic;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A dropdown menu/combobox.
  /// </summary>
  public sealed class NuiCombo : NuiWidget
  {
    public override string Type
    {
      get => "combo";
    }

    [JsonProperty("elements")]
    public NuiProperty<List<NuiComboEntry>> Entries { get; set; } = new List<NuiComboEntry>();

    [JsonProperty("value")]
    public NuiProperty<int> Selected { get; set; } = 0;
  }
}
