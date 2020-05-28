using System.Numerics;
using NWN;

namespace NWM.API
{
  public static class LocationFactory
  {
    public static Location Create(NwArea area, Vector3 position, float orientation)
    {
      return NWScript.Location(area, position.ToNativeVector(), orientation);
    }
  }
}