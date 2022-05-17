using System;
using System.Numerics;
using Newtonsoft.Json;

namespace Anvil.API
{
  public readonly struct NuiVector : IEquatable<NuiVector>
  {
    [JsonProperty("x")]
    public readonly float X;

    [JsonProperty("y")]
    public readonly float Y;

    [JsonConstructor]
    public NuiVector(float x, float y)
    {
      X = x;
      Y = y;
    }

    public static NuiVector operator +(NuiVector a, NuiVector b)
    {
      return new NuiVector(a.X + b.X, a.Y + b.Y);
    }

    public static bool operator ==(NuiVector left, NuiVector right)
    {
      return left.Equals(right);
    }

    public static implicit operator Vector2(NuiVector vector)
    {
      return new Vector2(vector.X, vector.Y);
    }

    public static implicit operator NuiVector(Vector2 vector)
    {
      return new NuiVector(vector.X, vector.Y);
    }

    public static bool operator !=(NuiVector left, NuiVector right)
    {
      return !left.Equals(right);
    }

    public static NuiVector operator -(NuiVector a, NuiVector b)
    {
      return new NuiVector(a.X - b.X, a.Y - b.Y);
    }

    public bool Equals(NuiVector other)
    {
      return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override bool Equals(object? obj)
    {
      return obj is NuiVector other && Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(X, Y);
    }
  }
}
