using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  internal sealed class NuiPropertyConverter : JsonConverterFactory
  {
    public override bool CanConvert(Type objectType)
    {
      if (!objectType.IsGenericType)
      {
        return false;
      }

      return objectType.GetGenericTypeDefinition() == typeof(NuiProperty<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
      Type valueType = typeToConvert.GetGenericArguments()[0];
      Type converterType = typeof(NuiPropertyConverterInner<>).MakeGenericType(valueType);

      return (JsonConverter?)Activator.CreateInstance(converterType,
        BindingFlags.Instance | BindingFlags.Public,
        null,
        [options],
        null);
    }

    private sealed class NuiPropertyConverterInner<TValue>(JsonSerializerOptions options) : JsonConverter<NuiProperty<TValue>>
    {
      private readonly JsonConverter<NuiBindStrRef> bindStrRefConverter = (JsonConverter<NuiBindStrRef>)options.GetConverter(typeof(NuiBindStrRef));
      private readonly JsonConverter<NuiValueStrRef> valueStrRefConverter = (JsonConverter<NuiValueStrRef>)options.GetConverter(typeof(NuiValueStrRef));
      private readonly JsonConverter<NuiBind<TValue>> bindConverter = (JsonConverter<NuiBind<TValue>>)options.GetConverter(typeof(NuiBind<TValue>));
      private readonly JsonConverter<NuiValue<TValue>> valueConverter = (JsonConverter<NuiValue<TValue>>)options.GetConverter(typeof(NuiValue<TValue>));

      public override NuiProperty<TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      {
        throw new NotImplementedException();
      }

      public override void Write(Utf8JsonWriter writer, NuiProperty<TValue> property, JsonSerializerOptions options)
      {
        switch (property)
        {
          case NuiBind<TValue> bind:
            bindConverter.Write(writer, bind, options);
            break;
          case NuiValue<TValue> value:
            valueConverter.Write(writer, value, options);
            break;
          case NuiBindStrRef bindStrRef:
            bindStrRefConverter.Write(writer, bindStrRef, options);
            break;
          case NuiValueStrRef valueStrRef:
            valueStrRefConverter.Write(writer, valueStrRef, options);
            break;
        }
      }
    }
  }
}
