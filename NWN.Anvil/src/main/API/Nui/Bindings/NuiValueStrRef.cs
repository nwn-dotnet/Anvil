using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A readonly NUI property value for localized strings (StrRef) that cannot be changed at runtime.
  /// </summary>
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

    /// <summary>
    /// Implicitly converts a readonly StrRef NUI property to its underlying value.
    /// </summary>
    /// <param name="value">The NUI StrRef value wrapper.</param>
    /// <returns>The underlying <see cref="StrRef"/>, or null.</returns>
    public static implicit operator StrRef?(NuiValueStrRef? value)
    {
      return value?.Value;
    }
  }
}
