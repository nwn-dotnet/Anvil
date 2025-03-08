using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A slider bar with floating-point values.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiSliderFloat(NuiProperty<float> value, NuiProperty<float> min, NuiProperty<float> max) : NuiWidget
  {
    [JsonProperty("max")]
    public NuiProperty<float> Max { get; set; } = max;

    [JsonProperty("min")]
    public NuiProperty<float> Min { get; set; } = min;

    [JsonProperty("step")]
    public NuiProperty<float> StepSize { get; set; } = 0.01f;

    public override string Type => "sliderf";

    [JsonProperty("value")]
    public NuiProperty<float> Value { get; set; } = value;
  }
}
