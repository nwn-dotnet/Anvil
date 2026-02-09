using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// JSON converter for <see cref="NuiValue{T}"/> that serializes/deserializes only the wrapped value.
  /// </summary>
  public sealed class NuiValueConverter : JsonConverter
  {
    /// <summary>
    /// Determines whether this converter can handle the specified type.
    /// </summary>
    public override bool CanConvert(Type objectType)
    {
      return objectType.GetGenericTypeDefinition() == typeof(NuiValue<>);
    }

    /// <summary>
    /// Reads a JSON value into a <see cref="NuiValue{T}"/> instance.
    /// </summary>
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
      object? retVal = Activator.CreateInstance(objectType, true);
      if (retVal == null)
      {
        return null;
      }

      PropertyInfo? propertyInfo = objectType.GetProperty(nameof(NuiValue<object>.Value));
      if (propertyInfo == null)
      {
        return null;
      }

      Type valueType = objectType.GetGenericArguments()[0];
      propertyInfo.SetValue(retVal, serializer.Deserialize(reader, valueType));

      return retVal;
    }

    /// <summary>
    /// Writes the wrapped value from a <see cref="NuiValue{T}"/> to JSON.
    /// </summary>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
        return;
      }

      Type type = value.GetType();
      PropertyInfo? propertyInfo = type.GetProperty(nameof(NuiValue<object>.Value));

      if (propertyInfo == null)
      {
        writer.WriteNull();
        return;
      }

      serializer.Serialize(writer, propertyInfo.GetValue(value));
    }
  }
}
