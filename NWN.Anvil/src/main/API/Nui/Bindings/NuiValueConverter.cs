using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Anvil.API
{
  public sealed class NuiValueConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return objectType.GetGenericTypeDefinition() == typeof(NuiValue<>);
    }

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
