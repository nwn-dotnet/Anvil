using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A slider bar with integer values.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiSlider(NuiProperty<int> value, NuiProperty<int> min, NuiProperty<int> max) : NuiWidget
  {
    [JsonPropertyName("max")]
    public NuiProperty<int> Max { get; set; } = max;

    [JsonPropertyName("min")]
    public NuiProperty<int> Min { get; set; } = min;

    [JsonPropertyName("step")]
    public NuiProperty<int> Step { get; set; } = 1;

    public override string Type => "slider";

    [JsonPropertyName("value")]
    public NuiProperty<int> Value { get; set; } = value;
  }
}
