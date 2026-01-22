using System.Text.Json.Serialization;

namespace Anvil.API
{
  /// <summary>
  /// A special widget that just takes up layout space.<br/>
  /// Configure the space used with the Width and Height properties.
  /// </summary>
  public sealed class NuiSpacer : NuiWidget
  {
    [JsonPropertyName("type")]
    public override string Type => "spacer";
  }
}
