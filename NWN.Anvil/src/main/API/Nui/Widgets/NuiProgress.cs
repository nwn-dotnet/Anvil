using System.Text.Json.Serialization;

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
    [JsonPropertyName("value")]
    public NuiProperty<float> Value { get; set; } = value;
  }
}
