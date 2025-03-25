using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Anvil.API
{
  public sealed class NuiValueStrRefConverter : JsonConverter<NuiValueStrRef?>
  {
    public override NuiValueStrRef? ReadJson(JsonReader reader, Type objectType, NuiValueStrRef? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      StrRef? value = serializer.Deserialize<StrRef?>(reader);
      return value != null ? new NuiValueStrRef(value) : null;
    }

    public override void WriteJson(JsonWriter writer, NuiValueStrRef? value, JsonSerializer serializer)
    {
      if (value?.Value != null)
      {
        serializer.Serialize(writer, value.Value);
      }
      else
      {
        writer.WriteNull();
      }
    }
  }
}
