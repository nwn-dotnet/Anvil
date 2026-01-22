using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  internal sealed class NuiValueStrRefConverter : JsonConverter<NuiValueStrRef?>
  {
    public override NuiValueStrRef? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      JsonConverter<StrRef> valueConverter = (JsonConverter<StrRef>)options.GetConverter(typeof(StrRef));

      StrRef? value = valueConverter.Read(ref reader, typeof(StrRef?), options);
      return value != null ? new NuiValueStrRef(value) : null;
    }

    public override void Write(Utf8JsonWriter writer, NuiValueStrRef? value, JsonSerializerOptions options)
    {
      JsonConverter<StrRef> valueConverter = (JsonConverter<StrRef>)options.GetConverter(typeof(StrRef));

      if (value?.Value != null)
      {
        valueConverter.Write(writer, value.Value.Value, options);
      }
      else
      {
        writer.WriteNullValue();
      }
    }
  }
}
