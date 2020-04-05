using System.Collections.Generic;
using System.Numerics;
using NWM.API.Constants;
using NWN;

namespace NWM.API
{
  public sealed class NwArea : NwObject
  {
    internal NwArea(uint objectId) : base(objectId) {}

    public bool IsNaturalArea => NWScript.GetIsAreaNatural(this) == NWScript.AREA_NATURAL;
    public bool IsAboveGround => (AreaInfo) NWScript.GetIsAreaAboveGround(this) == AreaInfo.AboveGround;
    public bool IsNatural => (AreaInfo) NWScript.GetIsAreaNatural(this) == AreaInfo.Natural;

    public Vector2 AreaSize => new Vector2(NWScript.GetAreaSize((int) AreaSizeDimension.Width, this), NWScript.GetAreaSize((int) AreaSizeDimension.Height, this));

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