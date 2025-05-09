using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  internal sealed class NuiComboEntryConverter : JsonConverter<NuiComboEntry>
  {
    public override NuiComboEntry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      reader.Read();
      if (reader.TokenType != JsonTokenType.StartArray)
      {
        throw new JsonException();
      }

      string? label = reader.GetString();
      int value = reader.GetInt32();

      NuiComboEntry? retVal = null;
      if (label != null)
      {
        retVal = new NuiComboEntry(label, value);
      }

      reader.Read();
      if (reader.TokenType != JsonTokenType.EndArray)
      {
        throw new JsonException();
      }

      return retVal;
    }

    public override void Write(Utf8JsonWriter writer, NuiComboEntry value, JsonSerializerOptions options)
    {
      writer.WriteStartArray();
      writer.WriteStringValue(value.Label);
      writer.WriteNumberValue(value.Value);
      writer.WriteEndArray();
    }
  }
}
