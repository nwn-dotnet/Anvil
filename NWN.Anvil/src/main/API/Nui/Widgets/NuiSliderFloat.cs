using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A slider bar with floating-point values.
  /// </summary>
  public sealed class NuiSliderFloat : NuiWidget
  {
    [JsonConstructor]
    public NuiSliderFloat(NuiProperty<float> value, NuiProperty<float> min, NuiProperty<float> max)
    {
      Value = value;
      Min = min;
      Max = max;
    }

    [JsonProperty("max")]
    public NuiProperty<float> Max { get; set; }

    [JsonProperty("min")]
    public NuiProperty<float> Min { get; set; }

    [JsonProperty("step")]
    public NuiProperty<float> StepSize { get; set; } = 0.01f;

    public override string Type
    {
      get => "sliderf";
    }

    [JsonProperty("value")]
    public NuiProperty<float> Value { get; set; }
  }
}
