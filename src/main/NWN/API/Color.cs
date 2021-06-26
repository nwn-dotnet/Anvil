using System;
using NWN.Native.API;

namespace NWN.API
{
  /// <summary>
  /// Represents an 8 bit Color structure.
  /// </summary>
  public readonly struct Color
  {
    /// <summary>
    /// Gets the red value of this color as a byte (0-255).
    /// </summary>
    public readonly byte Red;

    /// <summary>
    /// Gets the green value of this color as a byte (0-255).
    /// </summary>
    public readonly byte Green;

    /// <summary>
    /// Gets the blue value of this color as a byte (0-255).
    /// </summary>
    public readonly byte Blue;

    /// <summary>
    /// Gets the alpha value of this color as a byte (0-255).
    /// </summary>
    public readonly byte Alpha;

    /// <summary>
    /// Constructs a new Color from the given rgba values.
    /// </summary>
    /// <param name="red">The red value.</param>
    /// <param name="green">The green value.</param>
    /// <param name="blue">The blue value.</param>
    /// <param name="alpha">The alpha value.</param>
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
    /// Gets the red value of this color as a float (0-1).
    /// </summary>
    public float RedF
    {
      get => Red / 255f;
    }

    /// <summary>
    /// Gets the green value of this color as a float (0-1).
    /// </summary>
    public float GreenF
    {
      get => Green / 255f;
    }

    /// <summary>
    /// Gets the blue value of this color as a float (0-1).
    /// </summary>
    public float BlueF
    {
      get => Blue / 255f;
    }

    /// <summary>
    /// Gets the alpha value of this color as a float (0-1).
    /// </summary>
    public float AlphaF
    {
      get => Alpha / 255f;
    }

    /// <summary>
    /// Returns the 3 character sequence token for this color, used in coloring text.<br/>
    /// This is mostly for internal use. Use <see cref="StringExtensions.ColorString"/> for formatting text with a certain color.
    /// </summary>
    /// <returns>The 3 character sequence token representing this color.</returns>
    public string ToColorToken()
    {
      const byte tokenMinVal = 1;
      ReadOnlySpan<byte> tokenBytes = stackalloc[] { Math.Max(Red, tokenMinVal), Math.Max(Green, tokenMinVal), Math.Max(Blue, tokenMinVal) };
      return StringHelper.Cp1252Encoding.GetString(tokenBytes);
    }

    /// <summary>
    /// Returns an integer that represents this color.
    /// </summary>
    public int ToInt()
    {
      ReadOnlySpan<byte> data = stackalloc[] { Alpha, Blue, Green, Red };
      return BitConverter.ToInt32(data);
    }
  }
}
