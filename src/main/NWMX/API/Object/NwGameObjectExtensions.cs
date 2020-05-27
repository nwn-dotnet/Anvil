using NWM.API;
using NWM.API.Constants;
using NWNX;

namespace NWMX.API
{
  public static class NwGameObjectExtensions
  {
    public static void AcquireItem(this NwGameObject gameObject, NwItem item)
    {
      ObjectPlugin.AcquireItem(gameObject, item);
    }

    public static bool CheckFit(this NwGameObject gameObject, NwItem item)
    {
      return CheckFit(gameObject, item.BaseItemType);
    }

    public static bool CheckFit(this NwGameObject gameObject, BaseItemType baseItemType)
    {
      return ObjectPlugin.CheckFit(gameObject, (int) baseItemType).ToBool();
    }
  }
}