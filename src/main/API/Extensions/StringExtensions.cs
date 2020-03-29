using System;
using System.Text;

namespace NWM.API
{
  public static class StringExtensions
  {
    public static int ToInt(this string intString)
    {
      return Convert.ToInt32(intString);
    }

    public static void AppendColored(this StringBuilder stringBuilder, string text, Color color)
    {
      stringBuilder.Append(ColorString(text, color));
    }

    public static string ColorString(this string input, Color color)
    {
      return $"<c{color.ToColorToken()}>{input}</c>";
    }
  }
}