using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  public sealed class NuiValueConverter : JsonConverterFactory
  {
    public override bool CanConvert(Type objectType)
    {
      if (!objectType.IsGenericType)
      {
        return false;
      }

      return objectType.GetGenericTypeDefinition() == typeof(NuiValue<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
      Type valueType = typeToConvert.GetGenericArguments()[0];
      Type converterType = typeof(NuiValueConverterInner<>).MakeGenericType(valueType);

      return (JsonConverter?)Activator.CreateInstance(converterType,
        BindingFlags.Instance | BindingFlags.Public,
        null,
        [options],
        null);
    }

    private sealed class NuiValueConverterInner<TValue> : JsonConverter<NuiValue<TValue>>
    {
      private readonly JsonConverter<TValue> valueConverter;
      private readonly Type valueType;

      public NuiValueConverterInner(JsonSerializerOptions options)
      {
        valueType = typeof(TValue);
        valueConverter = (JsonConverter<TValue>)options.GetConverter(valueType);
      }

      public override NuiValue<TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      {
        TValue? value = valueConverter.Read(ref reader, valueType, options);
        return new NuiValue<TValue>(value);
      }

      public override void Write(Utf8JsonWriter writer, NuiValue<TValue> value, JsonSerializerOptions options)
      {
        if (value.Value is not null)
        {
          valueConverter.Write(writer, value.Value, options);
        }
        else
        {
          writer.WriteNullValue();
        }
      }
    }
  }
}
