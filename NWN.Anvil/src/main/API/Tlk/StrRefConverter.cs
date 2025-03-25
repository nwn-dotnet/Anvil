using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  internal sealed class StrRefConverter : JsonConverter<StrRef>
  {
    public override StrRef Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      CheckToken(ref reader, JsonTokenType.StartObject);
      reader.Read();
      CheckToken(ref reader, JsonTokenType.PropertyName);

      string? propertyName = reader.GetString();
      if (propertyName != "strref")
      {
        throw new JsonException($"Expected property name 'strref', got '{propertyName}'");
      }

      reader.Read();
      StrRef retVal = new StrRef(reader.GetUInt32());

      reader.Read();
      CheckToken(ref reader, JsonTokenType.EndObject);

      return retVal;
    }

    public override void Write(Utf8JsonWriter writer, StrRef value, JsonSerializerOptions options)
    {
      writer.WriteStartObject();
      writer.WritePropertyName("strref");
      writer.WriteNumberValue(value.Id);
      writer.WriteEndObject();
    }

    private void CheckToken(ref Utf8JsonReader reader, JsonTokenType tokenType)
    {
      if (reader.TokenType != tokenType)
      {
        throw new JsonException($"Expected token type '{tokenType}', got '{reader.TokenType}'");
      }
    }
  }
}
