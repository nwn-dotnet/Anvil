using System.Text.Json.Serialization;

namespace Anvil.API
{
  [JsonConverter(typeof(NuiValueStrRefConverter))]
  public sealed class NuiValueStrRef : NuiProperty<string>
  {
    public NuiValueStrRef(StrRef? value)
    {
      Value = value;
    }

    internal NuiValueStrRef() {}

    /// <summary>
    /// Gets the value of this property.
    /// </summary>
    public StrRef? Value { get; init; }

    public static implicit operator StrRef?(NuiValueStrRef? value)
    {
      return value?.Value;
    }
  }
}
