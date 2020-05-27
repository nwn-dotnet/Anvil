using NWM.API;
using NWNX;

namespace NWMX.API
{
  public static class NwGameObjectExtensions
  {
    public static void AcquireItem(this NwGameObject gameObject, NwItem item)
    {
      ObjectPlugin.AcquireItem(gameObject, item);
    }
  }
}