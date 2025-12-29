using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Anvil.API
{
  /// <summary>
  /// String helpers for script names, object ID parsing, color tokens, and common conversions.
  /// </summary>
  public static class StringExtensions
  {
    private static readonly Regex StripColorsRegex = new Regex("<c.+?(?=>)>|<\\/c>");

    /// <summary>
    /// Appends the specified text wrapped in a NWN color token.
    /// </summary>
    /// <param name="stringBuilder">The string builder to append to.</param>
    /// <param name="text">The text to append.</param>
    /// <param name="color">The color to apply via token.</param>
    public static void AppendColored(this StringBuilder stringBuilder, string text, Color color)
    {
      stringBuilder.Append(ColorString(text, color));
    }

    /// <summary>
    /// Wraps the specified text in a NWN color token string.
    /// </summary>
    /// <param name="input">The text to wrap.</param>
    /// <param name="color">The color to apply via token.</param>
    /// <returns>The formatted color token string.</returns>
    public static string ColorString(this string input, Color color)
    {
      return $"<c{color.ToColorToken()}>{input}</c>";
    }

    /// <summary>
    /// Gets whether the specified script name is reserved by Anvil.
    /// </summary>
    /// <param name="scriptName">The script name to check.</param>
    /// <returns>True if the name is reserved, otherwise false.</returns>
    public static bool IsReservedScriptName(this string scriptName)
    {
      if (string.IsNullOrEmpty(scriptName))
      {
        return false;
      }

      string lowerName = scriptName.ToLower();
      return lowerName is ScriptConstants.GameEventScriptName or ScriptConstants.NWNXEventScriptName;
    }

    /// <summary>
    /// Validates a script name against engine constraints.
    /// </summary>
    /// <remarks>
    /// The following constraints apply to script names:<br/>
    /// - Script must be &lt;=16 characters.<br/>
    /// - Scripts must only use alphanumeric characters (Aa-Zz, 0-9), underscores (_), or hyphens (-).<br/>
    /// - Scripts must not use reserved names: <see cref="ScriptConstants.GameEventScriptName"/> or <see cref="ScriptConstants.NWNXEventScriptName"/>.
    /// </remarks>
    /// <param name="scriptName">The script name to validate.</param>
    /// <param name="allowEmpty">If true, an empty or null name is allowed.</param>
    /// <returns>True if the name is valid, otherwise false.</returns>
    public static bool IsValidScriptName(this string? scriptName, bool allowEmpty)
    {
      if (string.IsNullOrEmpty(scriptName))
      {
        return allowEmpty;
      }

      if (scriptName.Length > 16)
      {
        return false;
      }

      foreach (char c in scriptName)
      {
        if (!char.IsLetterOrDigit(c) && c != '_' && c != '-')
        {
          return false;
        }
      }

      return !scriptName.Equals(ScriptConstants.GameEventScriptName, StringComparison.OrdinalIgnoreCase) && !scriptName.Equals(ScriptConstants.NWNXEventScriptName, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc cref="ParseFloat(string,float)"/>
    public static float ParseFloat(this string floatString)
    {
      return float.Parse(floatString);
    }

    /// <summary>
    /// Parses the specified string as a float.
    /// </summary>
    /// <param name="floatString">The float string to parse.</param>
    /// <param name="defaultValue">If parsing fails, the value to return instead.</param>
    public static float ParseFloat(this string floatString, float defaultValue)
    {
      return float.TryParse(floatString, out float retVal) ? retVal : defaultValue;
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
    public static NwObject? ParseObject(this string objectIdString)
    {
      return uint.Parse(objectIdString, NumberStyles.HexNumber).ToNwObject();
    }

    /// <inheritdoc cref="ParseObject"/>
    public static T? ParseObject<T>(this string objectIdString) where T : NwObject
    {
      return uint.Parse(objectIdString, NumberStyles.HexNumber).ToNwObject<T>();
    }

    /// <summary>
    /// Tries to resolve the specified GameObject ID string to an object. A return value
    /// indicates whether the conversion succeeded or failed.<br/>
    /// This is the temporary ID created from <see cref="NwObject.ToString"/>. See <see cref="GuidExtensions.ToNwObject"/> to parse persistent UUIDs.
    /// </summary>
    /// <param name="objectIdString">The object ID string to parse.</param>
    /// <param name="result">When this method returns, contains the object referenced by
    /// the number contained in objectIdString, if the conversion succeeded, or null if
    /// the conversion failed. The conversion fails if the s parameter is null or System.String.Empty,
    /// is not in a format compliant with style, or represents an invalid object reference.</param>
    /// <returns>true if objectIdString was converted successfully; otherwise, false.</returns>
    public static bool TryParseObject(this string objectIdString, [NotNullWhen(true)] out NwObject? result)
    {
      if (uint.TryParse(objectIdString, NumberStyles.HexNumber, null, out uint res) && res.ToNwObject() is {} obj)
      {
        result = obj;
        return true;
      }

      result = null;
      return false;
    }

    /// <inheritdoc cref="TryParseObject"/>
    public static bool TryParseObject<T>(this string objectIdString, [NotNullWhen(true)] out T? result) where T : NwObject
    {
      if (uint.TryParse(objectIdString, NumberStyles.HexNumber, null, out uint res) && res.ToNwObject<T>() is {} obj)
      {
        result = obj;
        return true;
      }

      result = null;
      return false;
    }

    /// <summary>
    /// Reads up to the specified number of characters from the reader.
    /// </summary>
    /// <param name="stringReader">The reader to read from.</param>
    /// <param name="length">The maximum number of characters to read.</param>
    /// <returns>A string containing the characters read, which may be shorter than <paramref name="length"/> if the end is reached.</returns>
    public static string ReadBlock(this StringReader stringReader, int length)
    {
      char[] retVal = new char[length];

      int next;
      int i = 0;

      while (i < length && (next = stringReader.Read()) >= 0)
      {
        retVal[i] = (char)next;
        i++;
      }

      return new string(retVal, 0, i);
    }

    /// <summary>
    /// Reads characters until the specified character is encountered, without consuming it.
    /// </summary>
    /// <param name="stringReader">The reader to read from.</param>
    /// <param name="character">The delimiter character to stop at.</param>
    /// <returns>The characters read as a string.</returns>
    public static string ReadUntilChar(this StringReader stringReader, char character)
    {
      List<char> retVal = [];

      int next;
      while ((next = stringReader.Peek()) >= 0)
      {
        char c = (char)next;
        if (c == character)
        {
          break;
        }

        stringReader.Read();
        retVal.Add(c);
      }

      return new string(retVal.ToArray());
    }

    /// <summary>
    /// Advances the reader by the specified number of characters.
    /// </summary>
    /// <param name="stringReader">The reader to advance.</param>
    /// <param name="count">The number of characters to skip.</param>
    public static void Skip(this StringReader stringReader, int count)
    {
      for (int i = 0; i < count; i++)
      {
        stringReader.Read();
      }
    }

    /// <summary>
    /// Strip any color codes from a string.
    /// </summary>
    /// <param name="input">The string to strip of color.</param>
    /// <returns>The new string without any color codes.</returns>
    public static string StripColors(this string input)
    {
      return StripColorsRegex.Replace(input, string.Empty);
    }

    /// <summary>
    /// Encodes the specified byte array as a base64 string.
    /// </summary>
    /// <param name="data">The data to encode.</param>
    /// <returns>The base64-encoded string.</returns>
    public static string ToBase64EncodedString(this byte[] data)
    {
      return Convert.ToBase64String(data);
    }

    /// <summary>
    /// Converts a base64 string to a byte array.
    /// </summary>
    /// <param name="base64String">The base64-encoded string.</param>
    /// <returns>The decoded byte array.</returns>
    public static byte[] ToByteArray(this string base64String)
    {
      return Convert.FromBase64String(base64String);
    }

    /// <summary>
    /// Tries to parse a float from the provided string.
    /// </summary>
    /// <param name="floatString">The input string.</param>
    /// <param name="result">The parsed float value if successful.</param>
    /// <returns>True if parsing succeeded; otherwise false.</returns>
    public static bool TryParseFloat(this string floatString, out float result)
    {
      return float.TryParse(floatString, out result);
    }

    /// <summary>
    /// Tries to parse an integer from the provided string.
    /// </summary>
    /// <param name="intString">The input string.</param>
    /// <param name="result">The parsed integer value if successful.</param>
    /// <returns>True if parsing succeeded; otherwise false.</returns>
    public static bool TryParseInt(this string intString, out int result)
    {
      return int.TryParse(intString, out result);
    }

    /// <summary>
    /// Tries to parse an integer-based boolean (1/0) and convert to a managed boolean using NWScript semantics.
    /// </summary>
    /// <param name="intBoolString">The integer string to parse.</param>
    /// <param name="result">Outputs true for non-zero values, false for zero.</param>
    /// <returns>True if parsing succeeded; otherwise false.</returns>
    public static bool TryParseIntBool(this string intBoolString, out bool result)
    {
      bool retVal = int.TryParse(intBoolString, out int intResult);
      result = intResult.ToBool();

      return retVal;
    }
  }
}
