using System;
using System.Numerics;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// Represents a 2D vector in NUI coordinate space.
  /// </summary>
  /// <param name="x">The horizontal component.</param>
  /// <param name="y">The vertical component.</param>
  [method: JsonConstructor]
  public readonly struct NuiVector(float x, float y) : IEquatable<NuiVector>
  {
    /// <summary>
    /// Gets the horizontal component of the vector.
    /// </summary>
    [JsonProperty("x")]
    public readonly float X = x;

    /// <summary>
    /// Gets the vertical component of the vector.
    /// </summary>
    [JsonProperty("y")]
    public readonly float Y = y;

    /// <summary>
    /// Adds two <see cref="NuiVector"/> values.
    /// </summary>
    public static NuiVector operator +(NuiVector a, NuiVector b)
    {
      return new NuiVector(a.X + b.X, a.Y + b.Y);
    }

    /// <summary>
    /// Determines whether two <see cref="NuiVector"/> values are equal.
    /// </summary>
    public static bool operator ==(NuiVector left, NuiVector right)
    {
      return left.Equals(right);
    }

    /// <summary>
    /// Converts a <see cref="NuiVector"/> to a <see cref="Vector2"/>.
    /// </summary>
    public static implicit operator Vector2(NuiVector vector)
    {
      return new Vector2(vector.X, vector.Y);
    }

    /// <summary>
    /// Converts a <see cref="Vector2"/> to a <see cref="NuiVector"/>.
    /// </summary>
    public static implicit operator NuiVector(Vector2 vector)
    {
      return new NuiVector(vector.X, vector.Y);
    }

    /// <summary>
    /// Determines whether two <see cref="NuiVector"/> values are not equal.
    /// </summary>
    public static bool operator !=(NuiVector left, NuiVector right)
    {
      return !left.Equals(right);
    }

    /// <summary>
    /// Subtracts one <see cref="NuiVector"/> from another.
    /// </summary>
    public static NuiVector operator -(NuiVector a, NuiVector b)
    {
      return new NuiVector(a.X - b.X, a.Y - b.Y);
    }

    /// <summary>
    /// Indicates whether this vector is equal to another.
    /// </summary>
    public bool Equals(NuiVector other)
    {
      return X.Equals(other.X) && Y.Equals(other.Y);
    }

    /// <summary>
    /// Indicates whether this instance is equal to a specified object.
    /// </summary>
    public override bool Equals(object? obj)
    {
      return obj is NuiVector other && Equals(other);
    }

    /// <summary>
    /// Returns a hash code for this vector.
    /// </summary>
    public override int GetHashCode()
    {
      return HashCode.Combine(X, Y);
    }
  }
}
