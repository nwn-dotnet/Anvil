using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  internal sealed class NuiListTemplateCellConverter(JsonSerializerOptions options) : JsonConverter<NuiListTemplateCell>
  {
    private readonly JsonConverter<NuiElement> nuiElementConverter = (JsonConverter<NuiElement>)options.GetConverter(typeof(NuiElement));

    public override NuiListTemplateCell? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
      reader.Read();
      if (reader.TokenType != JsonTokenType.StartArray)
      {
        throw new JsonException();
      }

      NuiElement? element = nuiElementConverter.Read(ref reader, typeof(NuiElement), options);
      bool variableSize = reader.GetBoolean();
      float width = reader.GetSingle();

      NuiListTemplateCell? retVal = null;
      if (element != null)
      {
        retVal = new NuiListTemplateCell(element)
        {
          VariableSize = variableSize,
          Width = width,
        };
      }

      reader.Read();
      if (reader.TokenType != JsonTokenType.EndArray)
      {
        throw new JsonException();
      }

      return retVal;
    }

    public override void Write(Utf8JsonWriter writer, NuiListTemplateCell value, JsonSerializerOptions options)
    {
      writer.WriteStartArray();
      nuiElementConverter.Write(writer, value.Element, options);
      writer.WriteBooleanValue(value.VariableSize);
      writer.WriteNumberValue(value.Width);
    }
  }
}
