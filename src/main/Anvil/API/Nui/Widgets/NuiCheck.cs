using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A checkbox with a label to the right of it.
  /// </summary>
  public sealed class NuiCheck : NuiElement
  {
    public override string Type
    {
      get => "check";
    }

    [JsonConstructor]
    public NuiCheck(NuiProperty<string> label, NuiProperty<bool> selected)
    {
      Label = label;
      Selected = selected;
    }

    [JsonProperty("label")]
    public NuiProperty<string> Label { get; set; }

    [JsonProperty("value")]
    public NuiProperty<bool> Selected { get; set; }
  }
}
