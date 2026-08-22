using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// A readonly NUI property value that cannot be changed at runtime.
  /// </summary>
  /// <typeparam name="T">The type of value being assigned.</typeparam>
  [JsonConverter(typeof(NuiValueConverter))]
  public sealed class NuiValue<T> : NuiProperty<T>
  {
    public NuiValue(T value)
    {
      Value = value;
    }

    internal NuiValue() {}

    /// <summary>
    /// Gets the value of this property.
    /// </summary>
    public T? Value { get; init; }

    /// <summary>
    /// Implicitly converts a readonly NUI property to its underlying value.
    /// </summary>
    /// <param name="value">The NUI value wrapper.</param>
    /// <returns>The underlying value, or default if null.</returns>
    public static implicit operator T?(NuiValue<T>? value)
    {
      return value != null ? value.Value : default;
    }
  }
}
