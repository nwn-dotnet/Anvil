using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A slider bar with floating-point values.
  /// </summary>
  public sealed class NuiSliderFloat : NuiElement
  {
    public override string Type
    {
      get => "sliderf";
    }

    [JsonConstructor]
    public NuiSliderFloat(NuiProperty<float> value, NuiProperty<float> min, NuiProperty<float> max)
    {
      Value = value;
      Min = min;
      Max = max;
    }

    [JsonProperty("value")]
    public NuiProperty<float> Value { get; set; }

    [JsonProperty("min")]
    public NuiProperty<float> Min { get; set; }

    [JsonProperty("max")]
    public NuiProperty<float> Max { get; set; }

    [JsonProperty("step")]
    public NuiProperty<float> StepSize { get; set; } = 0.01f;
  }
}
