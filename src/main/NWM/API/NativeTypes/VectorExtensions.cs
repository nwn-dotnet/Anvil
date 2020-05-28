using System.Numerics;

namespace NWM.API
{
  public static class VectorExtensions
  {
    public static Vector3 ToVector3(this NWN.Vector vector)
    {
      Vector3 retVal;
      retVal.X = vector.x;
      retVal.Y = vector.y;
      retVal.Z = vector.z;

      return retVal;
    }

    public static NWN.Vector ToNativeVector(this Vector3 vector)
    {
      NWN.Vector retVal;
      retVal.x = vector.X;
      retVal.y = vector.Y;
      retVal.z = vector.Z;

      return retVal;
    }

    public static NWN.Vector? ToNativeVector(this Vector3? vector)
    {
      if (!vector.HasValue)
      {
        return null;
      }

      return ToNativeVector(vector.Value);
    }
  }
}