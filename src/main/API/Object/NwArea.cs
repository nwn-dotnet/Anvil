using System.Collections.Generic;
using NWN;

namespace NWM.API
{
  public class NwArea : NwObject
  {
    protected internal NwArea(uint objectId) : base(objectId) {}

    public IEnumerable<NwObject> AreaObjects
    {
      get
      {
        for (NwObject areaObj = NWScript.GetFirstObjectInArea(this).ToNwObject(); areaObj != null; areaObj = NWScript.GetNextObjectInArea(this).ToNwObject())
        {
          yield return areaObj;
        }
      }
    }
  }
}