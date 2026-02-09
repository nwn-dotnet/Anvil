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
    /// <summary>
    /// Gets or sets the display label for this entry.
    /// </summary>
    [JsonProperty(Order = 1)]
    public string Label { get; set; } = label;

    /// <summary>
    /// Gets or sets the value associated with this entry.
    /// </summary>
    [JsonProperty(Order = 2)]
    public int Value { get; set; } = value;
  }
}
