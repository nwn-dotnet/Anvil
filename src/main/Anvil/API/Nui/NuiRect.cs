using Newtonsoft.Json;

namespace Anvil.API
{
  public readonly struct NuiRect
  {
    [JsonProperty("x")]
    public float X { get; }

    [JsonProperty("y")]
    public float Y { get; }

    [JsonProperty("w")]
    public float Width { get; }

    [JsonProperty("h")]
    public float Height { get; }

    public NuiRect(float x, float y, float width, float height)
    {
      X = x;
      Y = y;
      Width = width;
      Height = height;
    }
  }
}
