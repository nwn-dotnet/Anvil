using System;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// JSON converter for <see cref="NuiValueStrRef"/> that serializes/deserializes the contained <see cref="StrRef"/>.
  /// </summary>
  public sealed class NuiValueStrRefConverter : JsonConverter<NuiValueStrRef?>
  {
    /// <summary>
    /// Reads a JSON value into a <see cref="NuiValueStrRef"/> instance.
    /// </summary>
    public override NuiValueStrRef? ReadJson(JsonReader reader, Type objectType, NuiValueStrRef? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
      StrRef? value = serializer.Deserialize<StrRef?>(reader);
      return value != null ? new NuiValueStrRef(value) : null;
    }

    /// <summary>
    /// Writes the wrapped <see cref="StrRef"/> from a <see cref="NuiValueStrRef"/> to JSON.
    /// </summary>
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
