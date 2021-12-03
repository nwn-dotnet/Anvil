using System;

namespace Anvil.API
{
  /// <summary>
  /// A vector with two 32 bit integer values.
  /// </summary>
  public readonly struct Vector2Int : IEquatable<Vector2Int>
  {
    public readonly int X;
    public readonly int Y;

    public Vector2Int(int x = 0, int y = 0)
    {
      X = x;
      Y = y;
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
      return new Vector2Int(a.X + b.X, a.Y + b.Y);
    }

    public static bool operator ==(Vector2Int left, Vector2Int right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Vector2Int left, Vector2Int right)
    {
      return !left.Equals(right);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
      return new Vector2Int(a.X - b.X, a.Y - b.Y);
    }

    public bool Equals(Vector2Int other)
    {
      return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
      return obj is Vector2Int other && Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(X, Y);
    }
  }
}
