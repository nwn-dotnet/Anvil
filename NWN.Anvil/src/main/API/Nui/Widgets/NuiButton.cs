using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A clickable button with text as the label.
  /// </summary>
  public sealed class NuiButton : NuiWidget
  {
    [JsonConstructor]
    public NuiButton(NuiProperty<string> label)
    {
      Label = label;
    }

    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; }

    public override string Type
    {
      get => "button";
    }
  }
}
