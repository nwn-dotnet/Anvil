using System.Globalization;
using System.Text;

namespace NWN.API
{
  public static class StringExtensions
  {
    public static bool TryParseFloat(this string floatString, out float result)
    {
      return float.TryParse(floatString, out result);
    }

    /// <inheritdoc cref="ParseFloat(string,float)"/>
    public static float ParseFloat(this string floatString)
    {
      return float.Parse(floatString);
    }

    /// <summary>
    /// Parses the specified string as an float.
    /// </summary>
    /// <param name="floatString">The float string to parse.</param>
    /// <param name="defaultValue">If parsing fails, the value to return instead.</param>
    public static float ParseFloat(this string floatString, float defaultValue)
    {
      return float.TryParse(floatString, out float retVal) ? retVal : defaultValue;
    }

    public static bool TryParseInt(this string intString, out int result)
    {
      return int.TryParse(intString, out result);
    }

    /// <inheritdoc cref="ParseInt(string,int)"/>
    public static int ParseInt(this string intString)
    {
      return int.Parse(intString);
    }

    /// <summary>
    /// Parses the specified string as an integer.
    /// </summary>
    /// <param name="intString">The integer string to parse.</param>
    /// <param name="defaultValue">If parsing fails, the value to return instead.</param>
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

    /// <inheritdoc cref="ParseIntBool(string,bool)"/>
    public static bool ParseIntBool(this string intBoolString)
    {
      return intBoolString.ParseInt().ToBool();
    }

    /// <summary>
    /// Parses the specified string as an integer based boolean (1 = true, 0 = false).
    /// </summary>
    /// <param name="intBoolString">The integer string to parse.</param>
    /// <param name="defaultValue">If parsing fails, the value to return instead.</param>
    public static bool ParseIntBool(this string intBoolString, bool defaultValue)
    {
      return int.TryParse(intBoolString, out int retVal) ? retVal.ToBool() : defaultValue;
    }

    /// <summary>
    /// Resolves the specified GameObject ID string to an object.<br/>
    /// This is the temporary ID created from <see cref="NwObject.ToString"/>. See <see cref="GuidExtensions.ToNwObject"/> to parse persistent UUIDs.
    /// </summary>
    /// <param name="objectIdString">The object ID string to parse.</param>
    /// <returns>The NwObject associated with the specified object ID.</returns>
    public static NwObject ParseObject(this string objectIdString)
    {
      return uint.Parse(objectIdString, NumberStyles.HexNumber).ToNwObject();
    }

    /// <inheritdoc cref="ParseObject"/>
    public static T ParseObject<T>(this string objectIdString) where T : NwObject
    {
      return uint.Parse(objectIdString, NumberStyles.HexNumber).ToNwObject<T>();
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
