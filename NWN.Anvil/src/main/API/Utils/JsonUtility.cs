using System.Text.Json;

namespace Anvil.API
{
  /// <summary>
  /// Utility methods for serializing/deserializing JSON data.
  /// </summary>
  public static class JsonUtility
  {
    /// <summary>
    /// Deserializes a JSON string.
    /// </summary>
    /// <param name="json">The JSON to deserialize.</param>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <returns>The deserialized object.</returns>
    public static T? FromJson<T>(string json)
    {
      return JsonSerializer.Deserialize<T>(json);
    }

    /// <summary>
    /// Serializes a value as JSON.
    /// </summary>
    /// <param name="value">The value to serialize.</param>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <returns>A JSON string representing the value.</returns>
    public static string ToJson<T>(T value)
    {
      return JsonSerializer.Serialize(value);
    }

    /// <summary>
    /// Deserializes a Json game engine structure.
    /// </summary>
    /// <param name="json">The json to deserialize.</param>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <returns>The deserialized object.</returns>
    internal static T? FromJson<T>(Json json)
    {
      return JsonSerializer.Deserialize<T>(json.Dump());
    }

    /// <summary>
    /// Serializes a value as a JSON engine structure.
    /// </summary>
    /// <param name="value">The value to serialize.</param>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <returns>A JSON engine structure representing the value.</returns>
    internal static Json ToJsonStructure<T>(T value)
    {
      string serialized = ToJson(value);
      return Json.Parse(serialized);
    }
  }
}
