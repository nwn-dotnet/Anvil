using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A generic progress bar.
  /// </summary>
  public sealed class NuiProgress : NuiWidget
  {
    [JsonConstructor]
    public NuiProgress(NuiProperty<float> value)
    {
      Value = value;
    }

    public override string Type
    {
      get => "progress";
    }

    /// <summary>
    /// The current value of this progress bar (0-1).
    /// </summary>
    [JsonProperty("value")]
    public NuiProperty<float> Value { get; set; }
  }
}
