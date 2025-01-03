using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A combo/list element for use in <see cref="NuiCombo"/>.
  /// </summary>
  [JsonConverter(typeof(ObjectToArrayConverter<NuiComboEntry>))]
  [method: JsonConstructor]
  public sealed class NuiComboEntry(string label, int value)
  {
    [JsonProperty(Order = 1)]
    public string Label { get; set; } = label;

    [JsonProperty(Order = 2)]
    public int Value { get; set; } = value;
  }
}
