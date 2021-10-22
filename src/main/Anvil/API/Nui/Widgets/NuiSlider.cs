using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A slider bar with integer values.
  /// </summary>
  public sealed class NuiSlider : NuiWidget
  {
    public override string Type
    {
      get => "slider";
    }

    [JsonConstructor]
    public NuiSlider(NuiProperty<int> value, NuiProperty<int> min, NuiProperty<int> max)
    {
      Value = value;
      Min = min;
      Max = max;
    }

    [JsonProperty("value")]
    public NuiProperty<int> Value { get; set; }

    [JsonProperty("min")]
    public NuiProperty<int> Min { get; set; }

    [JsonProperty("max")]
    public NuiProperty<int> Max { get; set; }

    [JsonProperty("step")]
    public NuiProperty<int> Step { get; set; } = 1;
  }
}
