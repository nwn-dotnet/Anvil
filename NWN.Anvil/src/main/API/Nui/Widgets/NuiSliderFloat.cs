using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A slider bar with floating-point values.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiSliderFloat(NuiProperty<float> value, NuiProperty<float> min, NuiProperty<float> max) : NuiWidget
  {
    [JsonPropertyName("max")]
    public NuiProperty<float> Max { get; set; } = max;

    [JsonPropertyName("min")]
    public NuiProperty<float> Min { get; set; } = min;

    [JsonPropertyName("step")]
    public NuiProperty<float> StepSize { get; set; } = 0.01f;

    public override string Type => "sliderf";

    [JsonPropertyName("value")]
    public NuiProperty<float> Value { get; set; } = value;
  }
}
