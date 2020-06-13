using System.Text;

namespace NWN.API
{
  public static class StringExtensions
  {
    public static bool TryParseFloat(this string floatString, out float result)
    {
      return float.TryParse(floatString, out result);
    }

    public static float ParseFloat(this string floatString)
    {
      return float.Parse(floatString);
    }

    public static float ParseFloat(this string floatString, float defaultValue)
    {
      return float.TryParse(floatString, out float retVal) ? retVal : defaultValue;
    }

    public static bool TryParseInt(this string intString, out int result)
    {
      return int.TryParse(intString, out result);
    }

    public static int ParseInt(this string intString)
    {
      return int.Parse(intString);
    }

    public static int ParseInt(this string intString, int defaultValue)
    {
      return int.TryParse(intString, out int retVal) ? retVal : defaultValue;
    }

    public static bool TryParseIntBool(this string intBoolString, out bool result)
    {
      bool retVal = int.TryParse(intBoolString, out int intResult);
      result = intResult.ToBool();

      return retVal;
    }

    public static bool ParseIntBool(this string intBoolString)
    {
      return intBoolString.ParseInt().ToBool();
    }

    public static bool ParseIntBool(this string intBoolString, bool defaultValue)
    {
      return int.TryParse(intBoolString, out int retVal) ? retVal.ToBool() : defaultValue;
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