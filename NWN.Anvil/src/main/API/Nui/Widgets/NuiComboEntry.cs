using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A combo/list element for use in <see cref="NuiCombo"/>.
  /// </summary>
  [JsonConverter(typeof(NuiComboEntryConverter))]
  public sealed class NuiComboEntry(string label, int value)
  {
    public string Label { get; set; } = label;
    public int Value { get; set; } = value;
  }
}
