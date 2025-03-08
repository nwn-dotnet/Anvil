using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A slider bar with integer values.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiSlider(NuiProperty<int> value, NuiProperty<int> min, NuiProperty<int> max) : NuiWidget
  {
    [JsonProperty("max")]
    public NuiProperty<int> Max { get; set; } = max;

    [JsonProperty("min")]
    public NuiProperty<int> Min { get; set; } = min;

    [JsonProperty("step")]
    public NuiProperty<int> Step { get; set; } = 1;

    public override string Type => "slider";

    [JsonProperty("value")]
    public NuiProperty<int> Value { get; set; } = value;
  }
}
