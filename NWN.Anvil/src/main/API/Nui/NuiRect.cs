using Newtonsoft.Json;

namespace Anvil.API
{
  [method: JsonConstructor]
  public readonly struct NuiRect(float x, float y, float width, float height)
  {
    [JsonProperty("h")]
    public float Height { get; } = height;

    [JsonProperty("w")]
    public float Width { get; } = width;

    [JsonProperty("x")]
    public float X { get; } = x;

    [JsonProperty("y")]
    public float Y { get; } = y;
  }
}
