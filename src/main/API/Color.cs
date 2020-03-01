using System;
using System.Text;

namespace NWM.API
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

    public Color(byte red, byte green, byte blue)
    {
      Red = red;
      Green = green;
      Blue = blue;
    }

    public string ToColorToken()
    {
      return ENCODING.GetString(new[] {Math.Max(Red, MIN_STR), Math.Max(Green, MIN_STR), Math.Max(Blue, MIN_STR)});
    }
  }
}