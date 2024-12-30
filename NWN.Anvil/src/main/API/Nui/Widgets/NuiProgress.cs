using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A generic progress bar.
  /// </summary>
  [method: JsonConstructor]
  public sealed class NuiProgress(NuiProperty<float> value) : NuiWidget
  {
    public override string Type => "progress";

    /// <summary>
    /// The current value of this progress bar (0-1).
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<float> Value { get; set; } = value;
  }
}
