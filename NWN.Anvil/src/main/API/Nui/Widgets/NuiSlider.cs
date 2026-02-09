using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A slider bar with integer values.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiSlider(NuiProperty<int> value, NuiProperty<int> min, NuiProperty<int> max) : NuiWidget
  {
    /// <summary>
    /// Gets or sets the maximum slider value.
    /// </summary>
    [JsonProperty("max")]
    public NuiProperty<int> Max { get; set; } = max;

    /// <summary>
    /// Gets or sets the minimum slider value.
    /// </summary>
    [JsonProperty("min")]
    public NuiProperty<int> Min { get; set; } = min;

    /// <summary>
    /// Gets or sets the step increment between slider values.
    /// </summary>
    [JsonProperty("step")]
    public NuiProperty<int> Step { get; set; } = 1;

    /// <summary>
    /// Gets the NUI widget type identifier for this element.
    /// </summary>
    public override string Type => "slider";

    /// <summary>
    /// Gets or sets the current slider value.
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<int> Value { get; set; } = value;
  }
}
