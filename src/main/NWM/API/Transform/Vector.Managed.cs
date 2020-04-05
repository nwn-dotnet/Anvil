using System.Numerics;

// ReSharper disable once CheckNamespace
namespace NWN
{
  public partial struct Vector
  {
    public static implicit operator Vector(Vector3 vector)
    {
      Vector retVal;
      retVal.x = vector.X;
      retVal.y = vector.Y;
      retVal.z = vector.Z;

      return retVal;
    }

    public static implicit operator Vector3(Vector vector)
    {
      Vector3 retVal;
      retVal.X = vector.x;
      retVal.Y = vector.y;
      retVal.Z = vector.z;

      return retVal;
    }
  }
}