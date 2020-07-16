using System;
using System.Text;

namespace NWN.API
{
  public readonly struct Color
  {
    private const byte MIN_STR = 1;

    private static readonly Encoding ENCODING = Encoding.GetEncoding("ISO-8859-1");

    public static readonly Color BLACK = new Color(0, 0, 0);
    public static readonly Color BLUE = new Color(0, 0, 255);
    public static readonly Color GREEN = new Color(0, 255, 0);
    public static readonly Color PINK = new Color(255, 170, 170);
    public static readonly Color RED = new Color(255, 0, 0);
    public static readonly Color ROSE = new Color(255, 150, 150);
    public static readonly Color WHITE = new Color(255, 255, 255);

    public readonly byte Red;
    public readonly byte Green;
    public readonly byte Blue;
    public readonly byte Alpha;

    public float RedF => Red / 255f;
    public float GreenF => Green / 255f;
    public float BlueF => Blue / 255f;
    public float AlphaF => Alpha / 255f;

    public Color(byte red, byte green, byte blue, byte alpha = 255)
    {
      Red = red;
      Green = green;
      Blue = blue;
      Alpha = alpha;
    }

    public Color(float red, float green, float blue, float alpha = 1.0f)
    {
      Red = (byte) ((red * 255) + 0.5);
      Green = (byte) ((green * 255) + 0.5);
      Blue = (byte) ((blue * 255) + 0.5);
      Alpha = (byte) ((alpha * 255) + 0.5);
    }

    public string ToColorToken()
    {
      return ENCODING.GetString(new[] {Math.Max(Red, MIN_STR), Math.Max(Green, MIN_STR), Math.Max(Blue, MIN_STR)});
    }

    public int ToHex()
    {
      ReadOnlySpan<byte> data = stackalloc[] {Red, Green, Blue, Alpha};
      return BitConverter.ToInt32(data);
    }
  }
}