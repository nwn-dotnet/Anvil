using System.Text.Json.Serialization;

namespace Anvil.API
{
  [method: JsonConstructor]
  public readonly struct NuiRect(float x, float y, float width, float height)
  {
    [JsonPropertyName("h")]
    public float Height { get; } = height;

    [JsonPropertyName("w")]
    public float Width { get; } = width;

    [JsonPropertyName("x")]
    public float X { get; } = x;

    [JsonPropertyName("y")]
    public float Y { get; } = y;
  }
}
