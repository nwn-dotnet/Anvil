using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  public sealed class NuiValueStrRefConverter(JsonSerializerOptions options) : JsonConverter<NuiValueStrRef?>
  {
    private readonly JsonConverter<StrRef> valueConverter = (JsonConverter<StrRef>)options.GetConverter(typeof(StrRef));

    public override NuiValueStrRef? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      StrRef? value = valueConverter.Read(ref reader, typeof(StrRef?), options);
      return value != null ? new NuiValueStrRef(value) : null;
    }

    public override void Write(Utf8JsonWriter writer, NuiValueStrRef? value, JsonSerializerOptions options)
    {
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
