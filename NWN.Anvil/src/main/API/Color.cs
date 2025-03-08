using System;
using Newtonsoft.Json;
using NWNX.NET.Native;

namespace Anvil.API
{
  /// <summary>
  /// A 8 bit Color structure.
  /// </summary>
  public readonly struct Color : IEquatable<Color>
  {
    /// <summary>
    /// Gets the alpha value of this color as a byte (0-255).
    /// </summary>
    [JsonProperty("a")]
    public readonly byte Alpha;

    /// <summary>
    /// Gets the blue value of this color as a byte (0-255).
    /// </summary>
    [JsonProperty("b")]
    public readonly byte Blue;

    /// <summary>
    /// Gets the green value of this color as a byte (0-255).
    /// </summary>
    [JsonProperty("g")]
    public readonly byte Green;

    /// <summary>
    /// Gets the red value of this color as a byte (0-255).
    /// </summary>
    [JsonProperty("r")]
    public readonly byte Red;

    /// <summary>
    /// Constructs a new Color from the given rgba values.
    /// </summary>
    /// <param name="red">The red value.</param>
    /// <param name="green">The green value.</param>
    /// <param name="blue">The blue value.</param>
    /// <param name="alpha">The alpha value.</param>
    [JsonConstructor]
    public Color(byte red, byte green, byte blue, byte alpha = 255)
    {
      Red = red;
      Green = green;
      Blue = blue;
      Alpha = alpha;
    }

    /// <summary>
    /// Constructs a new Color from the given rgba values.
    /// </summary>
    /// <param name="red">The red value.</param>
    /// <param name="green">The green value.</param>
    /// <param name="blue">The blue value.</param>
    /// <param name="alpha">The alpha value.</param>
    public Color(float red, float green, float blue, float alpha = 1.0f)
    {
      Red = (byte)(red * 255 + 0.5);
      Green = (byte)(green * 255 + 0.5);
      Blue = (byte)(blue * 255 + 0.5);
      Alpha = (byte)(alpha * 255 + 0.5);
    }

    /// <summary>
    /// Gets the alpha value of this color as a float (0-1).
    /// </summary>
    [JsonIgnore]
    public float AlphaF => Alpha / 255f;

    /// <summary>
    /// Gets the blue value of this color as a float (0-1).
    /// </summary>
    [JsonIgnore]
    public float BlueF => Blue / 255f;

    /// <summary>
    /// Gets the green value of this color as a float (0-1).
    /// </summary>
    [JsonIgnore]
    public float GreenF => Green / 255f;

    /// <summary>
    /// Gets the red value of this color as a float (0-1).
    /// </summary>
    [JsonIgnore]
    public float RedF => Red / 255f;

    /// <summary>
    /// Creates a Color structure from a 32-bit RGBA value.
    /// </summary>
    /// <param name="rgba">The 32-bit RGBA value.</param>
    /// <returns>A Color structure representing the RGBA value.</returns>
    public static unsafe Color FromRGBA(int rgba)
    {
      byte* colorPtr = (byte*)&rgba;
      return new Color(colorPtr[3], colorPtr[2], colorPtr[1], colorPtr[0]);
    }

    /// <summary>
    /// Creates a Color structure from an unsigned 32-bit RGBA value.
    /// </summary>
    /// <param name="rgba">The 32-bit RGBA value.</param>
    /// <returns>A Color structure representing the RGBA value.</returns>
    public static unsafe Color FromRGBA(uint rgba)
    {
      byte* colorPtr = (byte*)&rgba;
      return new Color(colorPtr[3], colorPtr[2], colorPtr[1], colorPtr[0]);
    }

    /// <summary>
    /// Creates a Color structure from a 32-bit RGBA hex string.
    /// </summary>
    /// <param name="rgbaHexString">The 32-bit RGBA hex string.</param>
    /// <returns>A Color structure representing the RGBA value.</returns>
    public static Color FromRGBA(string rgbaHexString)
    {
      return FromRGBA(Convert.ToInt32(rgbaHexString.Trim().TrimStart('#'), 16));
    }

    public static bool operator ==(Color left, Color right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Color left, Color right)
    {
      return !left.Equals(right);
    }

    public bool Equals(Color other)
    {
      return Alpha == other.Alpha && Blue == other.Blue && Green == other.Green && Red == other.Red;
    }

    public override bool Equals(object? obj)
    {
      return obj is Color other && Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Alpha, Blue, Green, Red);
    }

    /// <summary>
    /// Returns the 3 character sequence token for this color, used in coloring text.<br/>
    /// This is mostly for internal use. Use <see cref="StringExtensions.ColorString"/> for formatting text with a certain color.
    /// </summary>
    /// <returns>The 3 character sequence token representing this color.</returns>
    public string ToColorToken()
    {
      const byte tokenMinVal = 1;
      ReadOnlySpan<byte> tokenBytes = [Math.Max(Red, tokenMinVal), Math.Max(Green, tokenMinVal), Math.Max(Blue, tokenMinVal)];
      return StringUtils.Encoding.GetString(tokenBytes);
    }

    /// <summary>
    /// Gets the 32-bit RGBA value of this Color structure.
    /// </summary>
    /// <returns>The 32-bit RGBA value of this Color.</returns>
    public int ToRGBA()
    {
      ReadOnlySpan<byte> data = [Alpha, Blue, Green, Red];
      return BitConverter.ToInt32(data);
    }

    public override string ToString()
    {
      return $"R:{Red}, G:{Green}, B:{Blue}, A:{Alpha}";
    }

    /// <summary>
    /// Gets the unsigned 32-bit RGBA value of this Color structure.
    /// </summary>
    /// <returns>The 32-bit RGBA value of this Color.</returns>
    public uint ToUnsignedRGBA()
    {
      ReadOnlySpan<byte> data = [Alpha, Blue, Green, Red];
      return BitConverter.ToUInt32(data);
    }
  }
}
