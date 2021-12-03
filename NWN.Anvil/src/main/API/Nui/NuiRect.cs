using Newtonsoft.Json;

namespace Anvil.API
{
  public readonly struct NuiRect
  {
    [JsonConstructor]
    public NuiRect(float x, float y, float width, float height)
    {
      X = x;
      Y = y;
      Width = width;
      Height = height;
    }

    [JsonProperty("h")]
    public float Height { get; }

    [JsonProperty("w")]
    public float Width { get; }

    [JsonProperty("x")]
    public float X { get; }

    [JsonProperty("y")]
    public float Y { get; }
  }
}
