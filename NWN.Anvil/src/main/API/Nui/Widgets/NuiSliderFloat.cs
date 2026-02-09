using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A slider bar with floating-point values.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiSliderFloat(NuiProperty<float> value, NuiProperty<float> min, NuiProperty<float> max) : NuiWidget
  {
    /// <summary>
    /// Gets or sets the maximum slider value.
    /// </summary>
    [JsonProperty("max")]
    public NuiProperty<float> Max { get; set; } = max;

    /// <summary>
    /// Gets or sets the minimum slider value.
    /// </summary>
    [JsonProperty("min")]
    public NuiProperty<float> Min { get; set; } = min;

    /// <summary>
    /// Gets or sets the step increment between slider values.
    /// </summary>
    [JsonProperty("step")]
    public NuiProperty<float> StepSize { get; set; } = 0.01f;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "sliderf";

    /// <summary>
    /// Gets or sets the current slider value.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<float> Value { get; set; } = value;
  }
}
